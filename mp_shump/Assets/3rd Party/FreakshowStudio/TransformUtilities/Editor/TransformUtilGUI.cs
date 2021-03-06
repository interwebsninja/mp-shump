/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2011-2015
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public partial class TransformUtil : EditorWindow
{
	/// <summary>
	/// Show preferences pane or not
	/// </summary>
	private static bool m_ShowPrefs = true;

	/// <summary>
	/// Show controls pane or not
	/// </summary>
	private static bool m_ShowControls = false;

	/// <summary>
	/// Separate color for grid floors or not
	/// </summary>
	private static bool m_SeparateGridfloorColors = false;
	
	/// <summary>
	/// Nudge nudges in grid spacing or separate setting
	/// </summary>
	private static bool m_NudgeToGrid = true;
	
	/// <summary>
	/// Scroll-view position for editor gui
	/// </summary>
	private static Vector2 m_ScrollPosition = new Vector2(0.0f,0.0f);
	
	/// <summary>
	/// Unity GUI function, for preferences and controls in editor.
	/// 
	/// Note: Uses undocumented SceneView.lastActiveSceneView
	/// </summary>
	void OnGUI()
	{
		m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
		m_ShowPrefs = EditorGUILayout.Foldout(m_ShowPrefs, "Preferences");
		if (m_ShowPrefs)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical();

			// Grid Settings
			grid = EditorGUILayout.Vector3Field("Grid Size", grid);
			gridOffset = 
				EditorGUILayout.Vector3Field("Grid Offset", gridOffset);

			EditorGUILayout.Separator();
			
			// Nudge Settings
			m_NudgeToGrid = 
				EditorGUILayout.Toggle("Nudge Equals Grid Size", 
				                       m_NudgeToGrid);
			if (m_NudgeToGrid)
			{
				nudge = grid;
			}
			else
			{
				nudge = EditorGUILayout.Vector3Field("Nudge Amount", nudge);
			}
			
			nudgeSpace = 
				(Space) EditorGUILayout.EnumPopup("Nudge Space", nudgeSpace);
			
			EditorGUILayout.Separator();
			
			// Grid Controls
			showGridXZ = EditorGUILayout.Toggle("Show XZ Grid", showGridXZ);
			showGridYX = EditorGUILayout.Toggle("Show YX Grid", showGridYX);
			showGridYZ = EditorGUILayout.Toggle("Show YZ Grid", showGridYZ);
						
			gridLines = EditorGUILayout.IntField("Grid Lines", gridLines);
			
			EditorGUILayout.Separator();
			
			m_SeparateGridfloorColors = 
				EditorGUILayout.Toggle("Separate Grid Floor Colors", 
				                       m_SeparateGridfloorColors);
			
			if (m_SeparateGridfloorColors)
			{
				gridXZColor = 
					EditorGUILayout.ColorField("XZ Grid Color", gridXZColor);
				gridYXColor = 
					EditorGUILayout.ColorField("YX Grid Color", gridYXColor);
				gridYZColor = 
					EditorGUILayout.ColorField("YZ Grid Color", gridYZColor);
			}
			else
			{
				gridXZColor = gridYXColor = gridYZColor = 
					EditorGUILayout.ColorField("Grid Color", gridXZColor);
			}

			EditorGUILayout.Separator();
			
			// Rotation Settings
			rotateAmount = 
				EditorGUILayout.FloatField("Rotation Amount", rotateAmount);
			
			rotationSpace = 
				(Space) EditorGUILayout.EnumPopup("Rotation Space", 
				                                  rotationSpace);
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		
		m_ShowControls = EditorGUILayout.Foldout(m_ShowControls, "Controls");
		if (m_ShowControls)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical();
			
			// Nudge Controls
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Nudge X"))
			{
				NudgeX();
			}		
			if (GUILayout.Button("Nudge -X"))
			{
				NudgeNegX();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Nudge Y"))
			{
				NudgeY();
			}
			if (GUILayout.Button("Nudge -Y"))
			{
				NudgeNegY();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Nudge Z"))
			{
				NudgeZ();
			}			
			if (GUILayout.Button("Nudge -Z"))
			{
				NudgeNegZ();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();

			// Snap Controls
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Snap X"))
			{
				SnapX();
			}
			if (GUILayout.Button("Snap XZ"))
			{
				SnapXZ();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Snap Y"))
			{
				SnapY();
			}		
			if (GUILayout.Button("Snap XY"))
			{
				SnapXY();
			}	
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Snap Z"))
			{
				SnapZ();
			}	
			if (GUILayout.Button("Snap YZ"))
			{
				SnapYZ();
			}					
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Snap XYZ"))
			{
				SnapXYZ();
			}				
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();

			// Flip Controls
			EditorGUILayout.BeginHorizontal();
	
			if (GUILayout.Button("Flip X"))
			{
				FlipX();
			}
			
			if (GUILayout.Button("Flip Y"))
			{
				FlipY();
			}
			
			if (GUILayout.Button("Flip Z"))
			{
				FlipZ();
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			
			// Rotate Controls
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Yaw Right"))
			{
				YawRight();
			}			
			if (GUILayout.Button("Yaw Left"))
			{
				YawLeft();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Pitch Up"))
			{
				PitchUp();
			}		
			if (GUILayout.Button("Pitch Down"))
			{
				PitchDown();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Roll Left"))
			{
				RollLeft();
			}		
			if (GUILayout.Button("Roll Right"))
			{
				RollRight();
			}			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Reset Rotation"))
			{
				ResetRotation();
			}			
			EditorGUILayout.EndHorizontal();			

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();			
		}
		EditorGUILayout.EndScrollView();
		
		if (SceneView.lastActiveSceneView != null)
		{
			SceneView.lastActiveSceneView.Repaint();
		}
	}
}