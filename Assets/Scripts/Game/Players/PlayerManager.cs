using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.Game.Data.Characters;
using pdxpartyparrot.Game.Data.Players;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayerManager
    {
        bool PlayersImmune { get; }

        bool DebugInput { get; }

        CharacterBehaviorData PlayerBehaviorData { get; }

        IReadOnlyCollection<IPlayer> Players { get; }

        void RespawnPlayers();

        void DespawnPlayers();

        void DestroyPlayers();
    }

    public abstract class PlayerManager<T> : SingletonBehavior<T>, IPlayerManager where T : PlayerManager<T>
    {
        #region Debug

        [SerializeField]
        private bool _playersImmune;

        public bool PlayersImmune => _playersImmune;

        [SerializeField]
        private bool _debugInput;

        public bool DebugInput => _debugInput;

        #endregion

        [SerializeField]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;

        // TODO: what if we have different types of players?
        [SerializeField]
        private CharacterBehaviorData _playerBehaviorData;

        public CharacterBehaviorData PlayerBehaviorData => _playerBehaviorData;

        [SerializeField]
        private Actor _playerActorPrefab;

        private Actor PlayerActorPrefab => _playerActorPrefab;

        private IPlayer PlayerPrefab => (IPlayer)_playerActorPrefab;

        private readonly HashSet<IPlayer> _players = new HashSet<IPlayer>();

        public IReadOnlyCollection<IPlayer> Players => _players;

        public int PlayerCount => Players.Count;

        private GameObject _playerContainer;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            Assert.IsTrue(_playerBehaviorData is PlayerBehaviorData);

            _playerContainer = new GameObject("Players");

            if(null != PlayerPrefab) {
                Core.Network.NetworkManager.Instance.RegisterPlayerPrefab(PlayerPrefab.NetworkPlayer);
            }
            Core.Network.NetworkManager.Instance.ApprovalCheckSuccessEvent += ApprovalCheckSuccessEventHandler;

            InitDebugMenu();

            GameStateManager.Instance.RegisterPlayerManager(this);
        }

        protected override void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.UnregisterPlayerManager();
            }

            DestroyDebugMenu();

            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ApprovalCheckSuccessEvent -= ApprovalCheckSuccessEventHandler;
                Core.Network.NetworkManager.Instance.UnregisterPlayerPrefab();
            }

            Destroy(_playerContainer);
            _playerContainer = null;

            base.OnDestroy();
        }

        #endregion

        private void SpawnPlayer(ulong clientId)
        {
            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            Debug.Log($"Spawning player {clientId}...");

            SpawnPoint spawnPoint = SpawnManager.Instance.GetPlayerSpawnPoint(clientId);
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            NetworkPlayer player = Core.Network.NetworkManager.Instance.SpawnPlayer<NetworkPlayer>(clientId, _playerContainer.transform);
            if(null == player) {
                Debug.LogError("Failed to spawn network player!");
                return;
            }
            player.Initialize(clientId);

            if(!spawnPoint.SpawnPlayer((Actor)player.Player)) {
                Debug.LogError("Failed to spawn player!");
                return;
            }

            _players.Add(player.Player);
        }

        public bool RespawnPlayer(IPlayer player)
        {
            return RespawnPlayer(player, SpawnManager.Instance.GetPlayerSpawnPoint(player.NetworkPlayer.ClientId));
        }

        public bool RespawnPlayerRandom(IPlayer player)
        {
            return RespawnPlayer(player, SpawnManager.Instance.GetRandomPlayerSpawnPoint(player.NetworkPlayer.ClientId));
        }

        public bool RespawnPlayerNearest(IPlayer player)
        {
            return RespawnPlayer(player, SpawnManager.Instance.GetNearestPlayerSpawnPoint(player.NetworkPlayer.ClientId, player.Movement.Position));
        }

        public bool RespawnPlayer(IPlayer player, string tag)
        {
            return RespawnPlayer(player, SpawnManager.Instance.GetPlayerSpawnPoint(tag));
        }

        public bool RespawnPlayerRandom(IPlayer player, string tag)
        {
            return RespawnPlayer(player, SpawnManager.Instance.GetRandomPlayerSpawnPoint(tag));
        }

        public bool RespawnPlayerNearest(IPlayer player, string tag)
        {
            return RespawnPlayer(player, SpawnManager.Instance.GetNearestPlayerSpawnPoint(tag, player.Movement.Position));
        }

        private bool RespawnPlayer(IPlayer player, SpawnPoint spawnPoint)
        {
            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            Debug.Log($"Respawning player {player.Id} at spawn point {spawnPoint?.name ?? "Invalid spawnpoint"}");

            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return false;
            }
            return spawnPoint.ReSpawn((Actor)player);
        }

        public bool RespawnPlayerPosition(IPlayer player, Vector3 position)
        {
            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            Debug.Log($"Respawning player {player.Id} at position {position}");

            Actor actor = (Actor)player;

            Transform actorTransform = actor.transform;

            actorTransform.position = position;

            actor.gameObject.SetActive(true);

            return actor.OnReSpawn(null);
        }

        public void RespawnPlayers()
        {
            foreach(IPlayer player in _players) {
                RespawnPlayer(player);
            }
        }

        public void RespawnPlayers(string tag)
        {
            foreach(IPlayer player in _players) {
                RespawnPlayer(player, tag);
            }
        }

        // TODO: despawning / destroying is never touching the NetworkManager, which is probably wrong
        // but there's no way to despawn / destroy a single player for a connection, it's all or nothing
        // so... not sure what to do here

        public void DespawnPlayer(IPlayer player)
        {
            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            Debug.Log($"Despawning player {player.Id}");

            player.GameObject.SetActive(false);
        }

        public void DespawnPlayers()
        {
            if(PlayerCount < 1) {
                return;
            }

            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            foreach(IPlayer player in _players) {
                DespawnPlayer(player);
            }
        }

        // TODO: figure out how to work this in when players disconnect
        public void DestroyPlayer(IPlayer player, bool remove = true)
        {
            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            Debug.Log($"Destroying player {player.Id}");

#if !USE_NETWORKING
            Destroy(player.GameObject);
#endif

            if(remove) {
                _players.Remove(player);
            }
        }

        public void DestroyPlayers()
        {
            if(PlayerCount < 1) {
                return;
            }

            Assert.IsTrue(NetworkManager.Instance.IsServerActive());

            foreach(IPlayer player in _players) {
                DestroyPlayer(player, false);
            }

            _players.Clear();
        }

        public void ReclaimPlayer(IPlayer player)
        {
            player.GameObject.transform.SetParent(_playerContainer.transform);
        }

        #region Event Handlers

        private void ApprovalCheckSuccessEventHandler(object sender, ApprovalCheckSuccessEventArgs args)
        {
            SpawnPlayer(args.ClientId);
        }

        #endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.PlayerManager");
            _debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Players", GUI.skin.box);
                foreach(IPlayer player in _players) {
                    GUILayout.Label($"{player.Id} {player.Movement.Position}");
                }
                GUILayout.EndVertical();

                _playersImmune = GUILayout.Toggle(_playersImmune, "Players Immune");
                _debugInput = GUILayout.Toggle(_debugInput, "Debug Input");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
