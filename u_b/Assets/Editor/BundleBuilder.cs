using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BundleBuilder : Editor
{
    [MenuItem("GameTools/Build All Prefabs")]
    private static void buildAllPrefabs()
    {
        const string FOLDER = ("Assets/Prefabs/");
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        AssetBundleBuild abb = new AssetBundleBuild();
        abb.assetBundleName = "qixinpiaochong";
        abb.assetNames = new string[] { FOLDER + "qixinpiaochong.prefab", FOLDER + "qixinpiaochong_seed.prefab" };
        builds.Add(abb);


        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
    }

}
