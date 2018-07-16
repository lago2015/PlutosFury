using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CharacterSelector.Scripts
{
    public class CharacterManager : SingletonBase<CharacterManager>
    {
        public CharacterInfo[] Characters;
        public GameObject canvasComp;
        private int curIngameIndex;
        public GameObject SpawnPoint;

        private CharacterInfo _currentCharacterType = null;

        private CharacterInfo _currentCharacter = null;

        
        protected override void Init()
        {
            Persist = true;
            base.Init();
        }

        private void Awake()
        {
            curIngameIndex = PlayerPrefs.GetInt("PlayerCharacterIndex");
        }

        public void Start()
        {
            if (SpawnPoint != null)
            {
                SetCurrentCharacterType(curIngameIndex);
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
                SpawnPoint.transform.position, Quaternion.identity);

            MoveObjectToCanvas(_currentCharacterType);
            curIngameIndex = index;
        }

        void MoveObjectToCanvas(CharacterInfo currObject)
        {
            
            currObject.gameObject.transform.SetParent(canvasComp.transform, false);
            currObject.transform.position = SpawnPoint.transform.position;
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

        public void CreateCurrentCharacter()
        {
            _currentCharacter = Instantiate<CharacterInfo>(_currentCharacterType, 
                SpawnPoint.transform.position, Quaternion.identity);

            MoveObjectToCanvas(_currentCharacter);
            PlayerPrefs.SetInt("PlayerCharacterIndex", curIngameIndex);
            _currentCharacter.gameObject.SetActive(false);
            

            
        }

        public CharacterInfo GetCurrentCharacter()
        {
            return _currentCharacter;
        }
    }
}
