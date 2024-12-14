using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubtrackInfo
{
    public AnimationCurve coinsPath;
    public int numOfCoins;
    public int numOfObstacles;
}

public class GenerateTrack : MonoBehaviour
{
    [SerializeField] private List<SubtrackInfo> listSubtrack;
    [SerializeField] private List<SubtrackInfo> listSubtrackTraining;
    [SerializeField] private SessionManager sessionManager;
    [SerializeField] private GameObject subtrack;
    [SerializeField] private GameObject firstSubTrack;
    [SerializeField] private int initNumOfSubTrack = 2;

    public int count = 0;
    public int cstep = 0;
    public int round = 0;
    private int totalRound = 2;
    public bool isTraining;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 nextPosition = new(0,0,30);
        isTraining = GameSingleton.Instance.isTraining;
        count = 0;
        // generate the first subtracks 
        for (int i = 0; i < initNumOfSubTrack; i++)
        {
            GameObject st = Instantiate(subtrack, transform);
            st.transform.localPosition = nextPosition;
            nextPosition += new Vector3(0, 0, 30);
            if(isTraining){
                st.GetComponentInChildren<GenerateObstacles>().info = listSubtrackTraining[i];
            }else{
                st.GetComponentInChildren<GenerateObstacles>().info = listSubtrack[i];
            }
            GameObject fst = Instantiate(firstSubTrack, transform);
            fst.transform.localPosition = nextPosition;
            nextPosition += new Vector3(0, 0, 30);
            count++;
        }
    }

    // 
    public void GenerateSubTrack(Transform previousSubTrack){
        // generate a single subtrack every time this method is called
        
        if(isTraining)
        {
            count %= listSubtrackTraining.Count;
        }else
        {
            cstep++;
            if(cstep % listSubtrack.Count == 0){
                round++;
            }
            count %= listSubtrack.Count;
            if (round == totalRound){
                sessionManager.ChangeState(GameStatus.FINISH);
            }
        }
        
        //Debug.Log("generate new subtrack");
        GameObject st = Instantiate(subtrack, transform);
        st.transform.localPosition = previousSubTrack.position +  new Vector3(0, 0, 60 * initNumOfSubTrack);
        if(isTraining){
                st.GetComponentInChildren<GenerateObstacles>().info = listSubtrackTraining[count];
        }else{
            st.GetComponentInChildren<GenerateObstacles>().info = listSubtrack[count];
        }
        GameObject fst = Instantiate(firstSubTrack, transform);
        fst.transform.localPosition = st.transform.localPosition + new Vector3(0,0, 30);
        count++;
    }

    public void RestartSubTrack()
    {
        GameObject st = Instantiate(firstSubTrack, transform);
        st.transform.localPosition = new(0,0,0);
        Vector3 nextPosition = new(0,0,30);
        count = 0;
        // generate the subtracks 
        for (int i = 0; i < initNumOfSubTrack; i++)
        {
            st = Instantiate(subtrack, transform);
            st.transform.localPosition = nextPosition;
            nextPosition += new Vector3(0, 0, 30);
            st.GetComponentInChildren<GenerateObstacles>().info = listSubtrack[i];
            GameObject fst = Instantiate(firstSubTrack, transform);
            fst.transform.localPosition = nextPosition;
            nextPosition += new Vector3(0, 0, 30);
            count++;
        }
        
    }
}
