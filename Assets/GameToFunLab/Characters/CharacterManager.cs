using System;
using GameToFunLab.CharacterMovement;
using GameToFunLab.Core;
using UnityEngine;

namespace GameToFunLab.Characters
{
    public class CharacterManager
    {
        public enum CharacterType
        {
            None,
            Player,
            NPC,
            Monster
        }
        
        public DefaultCharacter CreateCharacter(CharacterType type, CharacterData data)
        {
            DefaultCharacter newCharacter = null;
            switch (type)
            {
                case CharacterType.Player:
                    // return new Player { Vnum = data.Vnum };
                    newCharacter = CreatePlayerCharacter(data);
                    break;
                // case CharacterType.NPC:
                //     return new NPC { Vnum = data.Vnum };
                // case CharacterType.Monster:
                //     return new Monster { Vnum = data.Vnum };
                default:
                    throw new ArgumentException("Invalid character type");
            }

            return newCharacter;
        }
        // 플레이어 캐릭터 생성
        private Player CreatePlayerCharacter(CharacterData data)
        {
            // 플레이어 캐릭터 프리팹을 스폰합니다. (여기서는 프리팹 로드를 가정)
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Characters/player");
            if (playerPrefab == null)
            {
                FgLogger.LogError("플레이어 프리팹이 없습니다.");
                return null;
            }
            GameObject playerObject = GameObject.Instantiate(playerPrefab);
        
            // PlayerCharacter 설정
            Player player = playerObject.GetComponent<Player>();
            if (player == null)
            {
                FgLogger.LogError("플레이어 스크립트가 프리팹에 없습니다.");
                return null;
            }

            return player;
        }
    }
}