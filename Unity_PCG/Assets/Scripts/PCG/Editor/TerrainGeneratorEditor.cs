﻿using MED10.Utilities;
using UnityEditor;
using UnityEngine;
using EditorGUITable;

namespace MED10.PCG
{
    [CustomEditor(typeof(TerrainGenerator))]
    [CanEditMultipleObjects]
    public class TerrainGeneratorEditor : Editor
    {
        /*
         * Use Serialized properties instead of direct access  to public variables.
         * This way changes in the editor will be saved when we change and recompile the code. 
         */

        // properties
        private SerializedProperty seedType;
        private SerializedProperty fixedSeed;

        private SerializedProperty randomHeightRange;
        private SerializedProperty heightMapScale;
        private SerializedProperty heightMapImage;
        private SerializedProperty perlinScaleX;
        private SerializedProperty perlinScaleY;
        private SerializedProperty perlinOffsetX;
        private SerializedProperty perlinOffsetY;
        private SerializedProperty perlinOctaves;
        private SerializedProperty perlinPersistance;
        private SerializedProperty perlinHeightScale;

        private GUITableState perlinParameterTable;
        private SerializedProperty perlinParameters;
        private SerializedProperty resetTerrain;

        private SerializedProperty voronoiPeakCount;
        private SerializedProperty voronoiFallOff;
        private SerializedProperty voronoiDropOff;
        private SerializedProperty voronoiMinHeight;
        private SerializedProperty voronoiMaxHeight;
        private SerializedProperty voronoiType;

        private SerializedProperty MPminHeight;
        private SerializedProperty MPmaxHeight;
        private SerializedProperty MProughness;
        private SerializedProperty MPheightDampener;

        private SerializedProperty smoothAmount;

        private GUITableState splatMapTable;
        private SerializedProperty splatHeights;

        private GUITableState vegetationTable;
        private SerializedProperty vegetationData;
        private SerializedProperty maxTrees;
        private SerializedProperty treeSpacing;

        private GUITableState detailTable;
        private SerializedProperty details;
        private SerializedProperty maxDetails;
        private SerializedProperty detailSpacing;

        private SerializedProperty waterHeight;
        private SerializedProperty waterGO;
        private SerializedProperty shorelineMaterial;


        private SerializedProperty numberOfClouds;
        private SerializedProperty particlesPerCloud;
        private SerializedProperty cloudParticleSize;
        private SerializedProperty cloudMinSize;
        private SerializedProperty cloudMaxSize;
        private SerializedProperty cloudMaterial;
        private SerializedProperty cloudShadowMaterial;
        private SerializedProperty color;
        private SerializedProperty lining;
        private SerializedProperty minSpeed;
        private SerializedProperty maxSpeed;
        private SerializedProperty distanceTravelled;

        private Texture2D texture;
        // foldouts
        private bool showRandom = false;
        private bool showLoadHeights = false;
        private bool showPerlin = false;
        private bool showMultiplePerlin = false;
        private bool showVoroni = false;
        private bool showMidpointDisplacement = false;
        private bool showSmoothing = false;
        private bool showSplatMaps = false;
        private bool showHeightMap = false;
        private bool showVegetation = false;
        private bool showDetail = false;
        private bool showWater = false;
        private bool showErosion = false;
        private bool showClouds = false;

        //scroll bar
        private Vector2 scrollPos;


        void OnEnable()
        {
            resetTerrain = serializedObject.FindProperty("resetTerrain");

            seedType = serializedObject.FindProperty("seedType");
            fixedSeed = serializedObject.FindProperty("fixedSeed");


            randomHeightRange = serializedObject.FindProperty("randomHeightRange");
            heightMapScale = serializedObject.FindProperty("heightMapScale");
            heightMapImage = serializedObject.FindProperty("heightMapImage");

            perlinScaleX = serializedObject.FindProperty("perlinScaleX");
            perlinScaleY = serializedObject.FindProperty("perlinScaleY");
            perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
            perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
            perlinOctaves = serializedObject.FindProperty("perlinOctaves");
            perlinPersistance = serializedObject.FindProperty("perlinPersistance");
            perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");

            perlinParameterTable = new GUITableState("perlinParameterTable");
            perlinParameters = serializedObject.FindProperty("perlinParameters");

            voronoiPeakCount = serializedObject.FindProperty("voronoiPeakCount");
            voronoiFallOff = serializedObject.FindProperty("voronoiFallOff");
            voronoiDropOff = serializedObject.FindProperty("voronoiDropOff");
            voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
            voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
            voronoiType = serializedObject.FindProperty("voronoiType");

            MPminHeight = serializedObject.FindProperty("MPminHeight");
            MPmaxHeight = serializedObject.FindProperty("MPmaxHeight");
            MProughness = serializedObject.FindProperty("MProughness");
            MPheightDampener = serializedObject.FindProperty("MPheightDampener");

            smoothAmount = serializedObject.FindProperty("smoothAmount");

            splatMapTable = new GUITableState("splatMapTable");
            splatHeights = serializedObject.FindProperty("splatHeights");

            vegetationTable = new GUITableState("vegetationTable");
            vegetationData = serializedObject.FindProperty("vegetationData");
            maxTrees = serializedObject.FindProperty("maxTrees");
            treeSpacing = serializedObject.FindProperty("treeSpacing");

            detailTable = new GUITableState("details");
            details = serializedObject.FindProperty("details");
            maxDetails = serializedObject.FindProperty("maxDetails");
            detailSpacing = serializedObject.FindProperty("detailSpacing");

            waterHeight = serializedObject.FindProperty("waterHeight");
            waterGO = serializedObject.FindProperty("waterGO");
            shorelineMaterial = serializedObject.FindProperty("shorelineMaterial");

            numberOfClouds = serializedObject.FindProperty("numberOfClouds");
            particlesPerCloud = serializedObject.FindProperty("particlesPerCloud");
            cloudParticleSize = serializedObject.FindProperty("cloudParticleSize");
            cloudMinSize = serializedObject.FindProperty("cloudMinSize");
            cloudMaxSize = serializedObject.FindProperty("cloudMaxSize");
            cloudMaterial = serializedObject.FindProperty("cloudMaterial");
            cloudShadowMaterial = serializedObject.FindProperty("cloudShadowMaterial");
            color = serializedObject.FindProperty("color");
            lining = serializedObject.FindProperty("lining");
            minSpeed = serializedObject.FindProperty("minSpeed");
            maxSpeed = serializedObject.FindProperty("maxSpeed");
            distanceTravelled = serializedObject.FindProperty("distanceTravelled");
        }

        public override void OnInspectorGUI()
        {
            // Update values in GUI from Custom Terrain
            serializedObject.Update();

            TerrainGenerator terrain = (TerrainGenerator)target;

            EditorGUILayout.PropertyField(seedType);
            EditorGUILayout.PropertyField(fixedSeed);

            //Begin Scrollbar
            Rect r = EditorGUILayout.BeginVertical();
            scrollPos =
                EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(r.width), GUILayout.Height(r.height));
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(resetTerrain);

            showRandom = EditorGUILayout.Foldout(showRandom, "Random");
            if (showRandom)
            {
                HLine();
                GUILayout.Label("Set Heights Between Random Values", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(randomHeightRange);
                if (GUILayout.Button("Random Heights"))
                {
                    terrain.RandomTerrain();
                }
            }

            showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Load Heights");
            if (showLoadHeights)
            {
                HLine();
                GUILayout.Label("Set Heights From Image", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(heightMapScale);
                EditorGUILayout.PropertyField(heightMapImage);
                if (GUILayout.Button("Load Texture"))
                {
                    terrain.LoadTexture();
                }
            }

            showPerlin = EditorGUILayout.Foldout(showPerlin, "Perlin Noise");
            if (showPerlin)
            {
                GUILayout.Label("Set Heights From Perlin Noise", EditorStyles.boldLabel);
                EditorGUILayout.Slider(perlinScaleX, 0, 0.05f, new GUIContent("X Scale"));
                EditorGUILayout.Slider(perlinScaleY, 0, 0.05f, new GUIContent("Y Scale"));
                EditorGUILayout.IntSlider(perlinOffsetX, 0, 1000, new GUIContent("X Offset"));
                EditorGUILayout.IntSlider(perlinOffsetY, 0, 1000, new GUIContent("Y Offset"));
                EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
                EditorGUILayout.Slider(perlinPersistance, 0, 1, new GUIContent("Persistance"));
                EditorGUILayout.Slider(perlinHeightScale, 0, 1, new GUIContent("Height Scale"));


                if (GUILayout.Button("Perlin Noise"))
                {
                    terrain.Perlin();
                }
            }

            showMultiplePerlin = EditorGUILayout.Foldout(showMultiplePerlin, "Multiple Perlin");
            if (showMultiplePerlin)
            {
                HLine();
                GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);
                perlinParameterTable = GUITableLayout.DrawTable(perlinParameterTable,
                    perlinParameters);
                EditorGUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+"))
                {
                    Utils.AddNewData(ref terrain.perlinParameters);
                }
                if (GUILayout.Button("-"))
                {
                    Utils.RemoveData(ref terrain.perlinParameters);
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Apply Multiple Perlin"))
                {
                    terrain.MultiplePerlinTerrain();
                }
            }

            showVoroni = EditorGUILayout.Foldout(showVoroni, "Voronoi");
            if (showVoroni)
            {
                EditorGUILayout.PropertyField(voronoiType);
                EditorGUILayout.IntSlider(voronoiPeakCount, 0, 50, new GUIContent("Peak Count"));
                EditorGUILayout.Slider(voronoiFallOff, 0, 5, new GUIContent("Fall Off"));
                EditorGUILayout.Slider(voronoiDropOff, 0, 5, new GUIContent("Drop Off"));
                float minValue = voronoiMinHeight.floatValue;
                float maxValue = voronoiMaxHeight.floatValue;
                EditorGUILayout.LabelField("Peak Height Range");
                EditorGUILayout.LabelField("Min: " + minValue);
                EditorGUILayout.LabelField("Max: " + maxValue);
                EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0, 1);
                voronoiMaxHeight.floatValue = maxValue;
                voronoiMinHeight.floatValue = minValue;

                if (GUILayout.Button("Voronoi"))
                {
                    terrain.Voronoi();
                }
            }

            showMidpointDisplacement = EditorGUILayout.Foldout(showMidpointDisplacement, "Midpoint Displacement");
            if (showMidpointDisplacement)
            {

                EditorGUILayout.LabelField("Height Range");
                EditorGUILayout.PropertyField(MPminHeight, new GUIContent("Min Height"));
                EditorGUILayout.PropertyField(MPmaxHeight, new GUIContent("Max Height"));

                float minValue = MPminHeight.floatValue;
                float maxValue = MPmaxHeight.floatValue;
                EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, -1, 1);


                MPmaxHeight.floatValue = maxValue;
                MPminHeight.floatValue = minValue;

                //EditorGUILayout.PropertyField(MProughness, new GUIContent("Roughness"));
                EditorGUILayout.Slider(MProughness, 1.0f, 5.0f, new GUIContent("Roughness"));
                EditorGUILayout.Slider(MPheightDampener, 1.0f, 5.0f, new GUIContent("Height Damener"));
                //EditorGUILayout.PropertyField(MPheightDampener, new GUIContent("Height Dampener"));

                if (GUILayout.Button("MPD"))
                {
                    terrain.MidpointDisplacement();
                }
            }

            showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
            if (showSplatMaps)
            {
                HLine();
                GUILayout.Label("Splat Maps", EditorStyles.boldLabel);
                splatMapTable = GUITableLayout.DrawTable(splatMapTable, splatHeights);
                EditorGUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+"))
                {
                    Utils.AddNewData<Splatmap>(ref terrain.splatHeights);
                }
                if (GUILayout.Button("-"))
                {
                    Utils.RemoveData<Splatmap>(ref terrain.splatHeights);
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Apply Splatmaps"))
                {
                    terrain.SplatMaps();
                }
            }

            showVegetation = EditorGUILayout.Foldout(showVegetation, "Vegetation");
            if (showVegetation)
            {
                HLine();
                GUILayout.Label("Vegetation", EditorStyles.boldLabel);

                EditorGUILayout.IntSlider(maxTrees, 0, 100000, new GUIContent("Max Trees"));
                EditorGUILayout.IntSlider(treeSpacing, 1, 20, new GUIContent("Tree Spacing"));

                vegetationTable = GUITableLayout.DrawTable(vegetationTable, vegetationData);
                EditorGUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+"))
                {
                    Utils.AddNewData(ref terrain.vegetationData);
                }
                if (GUILayout.Button("-"))
                {
                    Utils.RemoveData<Vegetation>(ref terrain.vegetationData);
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Apply Vegetation"))
                {
                    terrain.PlantVegetation();
                }
            }

            showDetail = EditorGUILayout.Foldout(showDetail, "Details");
            if (showDetail)
            {
                HLine();
                GUILayout.Label("Details", EditorStyles.boldLabel);


                EditorGUILayout.IntSlider(maxDetails, 0, 10000, new GUIContent("Max Details"));
                EditorGUILayout.IntSlider(detailSpacing, 1, 20, new GUIContent("Detail Spacing"));

                detailTable = GUITableLayout.DrawTable(detailTable, details);

                terrain.GetComponent<Terrain>().detailObjectDistance = maxDetails.intValue;

                EditorGUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+"))
                {
                    Utils.AddNewData(ref terrain.details);
                }
                if (GUILayout.Button("-"))
                {
                    Utils.RemoveData<Detail>(ref terrain.details);
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Apply Details"))
                {
                    terrain.PaintDetails();
                }
            }

            showWater = EditorGUILayout.Foldout(showWater, "Water");
            if (showWater)
            {
                EditorGUILayout.Slider(waterHeight, 0, 1, new GUIContent("Water Height"));
                EditorGUILayout.PropertyField(waterGO, new GUIContent("Water Game Object"));

                if (GUILayout.Button("Add Water"))
                {
                    terrain.AddWater();
                }
                EditorGUILayout.PropertyField(shorelineMaterial);
                if (GUILayout.Button("Add Shore"))
                {
                    terrain.AddShore();
                }
            }

            showClouds = EditorGUILayout.Foldout(showClouds, "Clouds");
            if (showClouds)
            {
                EditorGUILayout.PropertyField(numberOfClouds);
                EditorGUILayout.PropertyField(particlesPerCloud);
                EditorGUILayout.PropertyField(cloudParticleSize);
                EditorGUILayout.PropertyField(cloudMinSize);
                EditorGUILayout.PropertyField(cloudMaxSize);
                EditorGUILayout.PropertyField(cloudMaterial);
                EditorGUILayout.PropertyField(cloudShadowMaterial);
                EditorGUILayout.PropertyField(color);
                EditorGUILayout.PropertyField(lining);
                EditorGUILayout.PropertyField(minSpeed);
                EditorGUILayout.PropertyField(maxSpeed);
                EditorGUILayout.PropertyField(distanceTravelled);
                if (GUILayout.Button("Generate Clouds"))
                {
                    terrain.GenerateClouds();
                }
            }

            HLine();

            showSmoothing = EditorGUILayout.Foldout(showSmoothing, "Smoothing");
            if (showSmoothing)
            {
                EditorGUILayout.IntSlider(smoothAmount, 1, 10, new GUIContent("Smooth Amount"));
                if (GUILayout.Button("Smooth"))
                {
                    terrain.Smooth();
                }
            }

            if (GUILayout.Button("Falloff"))
            {
                terrain.AddFalloffMap();
            }

            if (GUILayout.Button("Reset"))
            {
                terrain.ResetTerrain();
            }

            HLine();
            showHeightMap = EditorGUILayout.Foldout(showHeightMap, "Heightmap");
            if (showHeightMap)
            {
                if (texture == null)
                {
                    texture = new Texture2D(terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
                }
                int heightmapTextureSize = (int)(EditorGUIUtility.currentViewWidth - 100);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(texture, GUILayout.Width(heightmapTextureSize), GUILayout.Height(heightmapTextureSize));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Refresh", GUILayout.Width(heightmapTextureSize)))
                {
                    float[,] heightMap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
                    for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
                    {
                        for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
                        {
                            texture.SetPixel(x, y, new Color(heightMap[x, y],
                                heightMap[x, y],
                                heightMap[x, y],
                                1));
                        }
                    }
                    texture.Apply();
                }
            }
            // End Scrollbar
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            // Apply changes to Custom Terrain
            serializedObject.ApplyModifiedProperties();
        }

        private static void HLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

    }

}