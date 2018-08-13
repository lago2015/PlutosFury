using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace CharacterSelector.Scripts
{
    public class CharacterManager : SingletonBase<CharacterManager>
    {
        public Image[] plutoSprites;
        public Image[] moonballSprites;

        public CharacterInfo[] Characters;
        public CharacterInfo[] Moonballs;
        public GameObject canvasComp;
        private int curIngameIndex;
        private int curBallIndex;
        public GameObject CharacterSpawnPoint;
        public GameObject MoonballSpawnPoint;
        private CharacterInfo _currentCharacterType = null;
        private CharacterInfo _currentMoonballType = null;
        private CharacterInfo _currentCharacter = null;
        private CharacterInfo _currentMoonball = null;

        protected override void Init()
        {
            Persist = true;
            base.Init();
        }

        private void Awake()
        {
            CheckSprites();
        }

        public void CheckSprites()
        {
            curIngameIndex = PlayerPrefs.GetInt("PlayerCharacterIndex");
            curBallIndex = PlayerPrefs.GetInt("PlayerMoonballIndex");
            for (int i = 0; i <= plutoSprites.Length - 1; i++)
            {
                if (i == curIngameIndex)
                {
                    plutoSprites[i].enabled = true;
                }
                else
                {
                    plutoSprites[i].enabled = false;
                }
            }

            for (int i = 0; i <= moonballSprites.Length - 1; i++)
            {
                if (i == curBallIndex)
                {
                    moonballSprites[i].enabled = true;
                }
                else
                {
                    moonballSprites[i].enabled = false;
                }
            }
        }

        public void Start()
        {
            if (CharacterSpawnPoint != null)
            {
                SetCurrentCharacterType(curIngameIndex);
            }
            if (MoonballSpawnPoint != null)
            {
                SetCurrentMoonballType(curBallIndex);
            }
        }

        public void SetCurrentCharacterType(int index)
        {
            if(_currentCharacterType != null)
            {
                Destroy(_currentCharacterType.gameObject);
            }

            CharacterInfo character = Characters[index];
            _currentCharacterType = Instantiate<CharacterInfo>(character, 
                CharacterSpawnPoint.transform.position, Quaternion.identity);
            MoveObjectToCanvas(_currentCharacterType);

            curIngameIndex = index;
        }


        public void SetCurrentMoonballType(int index)
        {
            if (_currentMoonballType != null)
            {
                Destroy(_currentMoonballType.gameObject);
            }

            CharacterInfo Moonball = Moonballs[index];
            _currentMoonballType = Instantiate<CharacterInfo>(Moonball,
                MoonballSpawnPoint.transform.position, Quaternion.identity);
            MoveMoonballToCanvas(_currentMoonballType);

            curBallIndex = index;
        }
        void MoveObjectToCanvas(CharacterInfo currObject)
        {
            
            currObject.gameObject.transform.SetParent(canvasComp.transform, false);
            currObject.transform.position = CharacterSpawnPoint.transform.position;
        }

        void MoveMoonballToCanvas(CharacterInfo currObject)
        {

            currObject.gameObject.transform.SetParent(canvasComp.transform, false);
            currObject.transform.position = MoonballSpawnPoint.transform.position;
        }

        public void SetCurrentCharacterType(string name,int curIndex)
        {
            int idx = 0;
            curIngameIndex = curIndex;
            foreach(CharacterInfo characterInfo in Characters)
            {
                if (characterInfo.CharacterType.Equals(name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    SetCurrentCharacterType(idx);
                    break;
                }
                idx++;
            }
        }
        public void SetCurrentMoonballType(string name, int curIndex)
        {
            int idx = 0;
            curBallIndex = curIndex;
            foreach (CharacterInfo characterInfo in Moonballs)
            {
                if (characterInfo.CharacterType.Equals(name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    SetCurrentCharacterType(idx);
                    break;
                }
                idx++;
            }
        }
        public void CreateCurrentCharacter()
        {
            PlayerPrefs.SetInt("PlayerCharacterIndex", curIngameIndex);
         
        }

        public void CreateCurrentMoonball()
        {
            PlayerPrefs.SetInt("PlayerMoonballIndex", curBallIndex);
        }
        public CharacterInfo GetCurrentCharacter()
        {
            return _currentCharacter;
        }

        public CharacterInfo GetCurrentMoonball()
        {
            return _currentMoonball;
        }
    }
}
