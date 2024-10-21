using Spine2d;
using UnityEngine;

public class UISpineMetadata
{
    public UISpineManager.UISpineVnum SpineVnum = UISpineManager.UISpineVnum.None;
    public bool Loop = false;
    public Vector2 Position = Vector2.zero;
}
public class UISpineManager : MonoBehaviour
{
    public enum UISpineVnum 
    {
        None,
        SpineBackendLoadingIcon,
        SpineUpgradeAtkButton,
    }

    public Spine2dUIController[] uiSpines;

    private void Start()
    {
        foreach (var spine2dUIController in uiSpines)
        {
            if (spine2dUIController == null) continue;
            spine2dUIController.StopAnimation();
            spine2dUIController.gameObject.SetActive(false);
        }
    }
    private void ShowSpineByVnum(bool show, UISpineMetadata uiSpineMetadata)
    {
        UISpineVnum vnum = uiSpineMetadata.SpineVnum;
        if (vnum == UISpineVnum.None) return;
        Spine2dUIController spine2dUIController = uiSpines[(int)vnum];
        if (spine2dUIController == null) return;
            
        if (show)
        {
            bool loop = uiSpineMetadata.Loop;
            Vector2 position = uiSpineMetadata.Position;
            spine2dUIController.gameObject.SetActive(true);
            spine2dUIController.PlayAnimation("play", loop);
            if (position != Vector2.zero)
            {
                spine2dUIController.gameObject.transform.position = position;
            }
        }
        else
        {
            spine2dUIController.StopAnimation();
            spine2dUIController.gameObject.SetActive(false);
        }
    }
}