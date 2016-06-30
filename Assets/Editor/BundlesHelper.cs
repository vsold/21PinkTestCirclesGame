using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{
        Debug.Log("Application.dataPath = " + Application.dataPath + "AssetBundles");
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundles/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}
}