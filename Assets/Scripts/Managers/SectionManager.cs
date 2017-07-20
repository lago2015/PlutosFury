using UnityEngine;
using System.Collections;

public class SectionManager : MonoBehaviour {

    public GameObject[] sections;
    public GameObject[] sectionExitLocations;

    private bool currentlyChanging;
    public int currSectionNumber;
    public bool isChanging(bool isSectiongChanging) { return currentlyChanging = isSectiongChanging; }
    public int NumOfSections() { return sections.Length; }
    void Awake()
    {
        for(int i=0;i<sections.Length;++i)
        {
            if(i==0)
            {
                sections[i].SetActive(true);

            }
            else if (sections[i])
            {
                sections[i].SetActive(false);
            }
        }
    }

    public void ChangeSection(GameObject curDoor)
    {
        if(currentlyChanging)
        {

            Door currentDoor = curDoor.GetComponent<Door>();
            if (currSectionNumber < sections.Length-1)
            {
                GameObject curGameobject = sectionExitLocations[currSectionNumber];
                Vector3 nextLocation = curGameobject.transform.position;
                currentDoor.StartCoroutine(currentDoor.ChangeScene(nextLocation));

                //Check if there is a next section location and if current section number hasnt gone over array length
                if (nextLocation != Vector3.zero && currSectionNumber < sections.Length)
                {

                    //check what section the player is in
                    if (sections[currSectionNumber])
                    {

                        sections[currSectionNumber].SetActive(false);
                        currSectionNumber++;
                        if (sections[currSectionNumber])
                        {
                            sections[currSectionNumber].SetActive(true);

                        }
                    }
                }
                //if current section number is maxed according to array length
                else if (currSectionNumber == sections.Length)
                {
                    //do nothing... da fuck?
                }
            }
        }   
            
    }
    
}
