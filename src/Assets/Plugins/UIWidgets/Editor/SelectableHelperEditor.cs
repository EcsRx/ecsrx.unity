﻿using UnityEngine;
using UnityEngine.UI;
using UnityEditor.AnimatedValues;
using UnityEditor;
using UnityEditor.Animations;


namespace UIWidgets {
	[CustomEditor(typeof(SelectableHelper), true)]
	public class SelectableHelperEditor : Editor
	{
		SerializedProperty m_Script;
		SerializedProperty m_TargetGraphicProperty;
		SerializedProperty m_TransitionProperty;
		SerializedProperty m_ColorBlockProperty;
		SerializedProperty m_SpriteStateProperty;
		SerializedProperty m_AnimTriggerProperty;

		AnimBool m_ShowColorTint = new AnimBool();
		AnimBool m_ShowSpriteTrasition = new AnimBool();
		AnimBool m_ShowAnimTransition = new AnimBool();

		string[] m_PropertyPathToExcludeForChildClasses;

		protected virtual void OnEnable()
		{
			m_Script                = serializedObject.FindProperty("m_Script");
			m_TargetGraphicProperty = serializedObject.FindProperty("targetGraphic");
			m_TransitionProperty    = serializedObject.FindProperty("transition");
			m_ColorBlockProperty    = serializedObject.FindProperty("colors");
			m_SpriteStateProperty   = serializedObject.FindProperty("spriteState");
			m_AnimTriggerProperty   = serializedObject.FindProperty("animationTriggers");

			m_PropertyPathToExcludeForChildClasses = new[] {
				m_Script.propertyPath,
				m_TransitionProperty.propertyPath,
				m_ColorBlockProperty.propertyPath,
				m_SpriteStateProperty.propertyPath,
				m_AnimTriggerProperty.propertyPath,
				m_TargetGraphicProperty.propertyPath,
			};

			var trans = GetTransition(m_TransitionProperty);
			m_ShowColorTint.value       = (trans == Selectable.Transition.ColorTint);
			m_ShowSpriteTrasition.value = (trans == Selectable.Transition.SpriteSwap);
			m_ShowAnimTransition.value  = (trans == Selectable.Transition.Animation);

			m_ShowColorTint.valueChanged.AddListener(Repaint);
			m_ShowSpriteTrasition.valueChanged.AddListener(Repaint);
		}

		protected virtual void OnDisable()
		{
			m_ShowColorTint.valueChanged.RemoveListener(Repaint);
			m_ShowSpriteTrasition.valueChanged.RemoveListener(Repaint);
		}

		static Selectable.Transition GetTransition(SerializedProperty transition)
		{
			return (Selectable.Transition)transition.enumValueIndex;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (!IsDerivedSelectableHelperEditor())
			{
				EditorGUILayout.PropertyField(m_Script);
			}

			var trans = GetTransition(m_TransitionProperty);

			var graphic = m_TargetGraphicProperty.objectReferenceValue as Graphic;
			if (graphic == null)
			{
				graphic = (target as SelectableHelper).GetComponent<Graphic>();
			}

			var animator = (target as SelectableHelper).GetComponent<Animator>();
			m_ShowColorTint.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Button.Transition.ColorTint);
			m_ShowSpriteTrasition.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Button.Transition.SpriteSwap);
			m_ShowAnimTransition.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Button.Transition.Animation);

			EditorGUILayout.PropertyField(m_TransitionProperty);

			++EditorGUI.indentLevel;
			{
				if (trans == Selectable.Transition.ColorTint || trans == Selectable.Transition.SpriteSwap)
				{
					EditorGUILayout.PropertyField(m_TargetGraphicProperty);
				}

				switch (trans)
				{
					case Selectable.Transition.ColorTint:
						if (graphic == null)
						{
							EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Warning);
						}
						break;

					case Selectable.Transition.SpriteSwap:
						if (graphic as Image == null)
						{
							EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", MessageType.Warning);
						}
						break;
				}

				if (EditorGUILayout.BeginFadeGroup(m_ShowColorTint.faded))
				{
					EditorGUILayout.PropertyField(m_ColorBlockProperty);
					EditorGUILayout.Space();
				}
				EditorGUILayout.EndFadeGroup();

				if (EditorGUILayout.BeginFadeGroup(m_ShowSpriteTrasition.faded))
				{
					EditorGUILayout.PropertyField(m_SpriteStateProperty);
					EditorGUILayout.Space();
				}
				EditorGUILayout.EndFadeGroup();

				if (EditorGUILayout.BeginFadeGroup(m_ShowAnimTransition.faded))
				{
					EditorGUILayout.PropertyField(m_AnimTriggerProperty);

					if (animator == null || animator.runtimeAnimatorController == null)
					{
						Rect buttonRect = EditorGUILayout.GetControlRect();
						buttonRect.xMin += EditorGUIUtility.labelWidth;
						if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
						{
							UnityEditor.Animations.AnimatorController controller = GenerateSelectableAnimatorContoller((target as SelectableHelper).AnimationTriggers, target as SelectableHelper);
							if (controller != null)
							{
								if (animator == null)
								{
									animator = (target as SelectableHelper).gameObject.AddComponent<Animator>();
								}

								UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, controller);
							}
						}
					}
				}
				EditorGUILayout.EndFadeGroup();
			}
			--EditorGUI.indentLevel;

			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			Rect toggleRect = EditorGUILayout.GetControlRect();
			toggleRect.xMin += EditorGUIUtility.labelWidth;

			ChildClassPropertiesGUI();

			serializedObject.ApplyModifiedProperties();
		}

		void ChildClassPropertiesGUI()
		{
			if (IsDerivedSelectableHelperEditor())
			{
				return;
			}

			DrawPropertiesExcluding(serializedObject, m_PropertyPathToExcludeForChildClasses);
		}

		bool IsDerivedSelectableHelperEditor()
		{
			return GetType() != typeof(SelectableHelperEditor);
		}

		static UnityEditor.Animations.AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, SelectableHelper target)
		{
			if (target == null)
			{
				return null;
			}

			var path = GetSaveControllerPath(target);
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}

			var normalName = string.IsNullOrEmpty(animationTriggers.normalTrigger) ? "Normal" : animationTriggers.normalTrigger;
			var highlightedName = string.IsNullOrEmpty(animationTriggers.highlightedTrigger) ? "Highlighted" : animationTriggers.highlightedTrigger;
			var pressedName = string.IsNullOrEmpty(animationTriggers.pressedTrigger) ? "Pressed" : animationTriggers.pressedTrigger;
			var disabledName = string.IsNullOrEmpty(animationTriggers.disabledTrigger) ? "Disabled" : animationTriggers.disabledTrigger;

			var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path);

			GenerateTriggerableTransition(normalName, controller);
			GenerateTriggerableTransition(highlightedName, controller);
			GenerateTriggerableTransition(pressedName, controller);
			GenerateTriggerableTransition(disabledName, controller);

			return controller;
		}

		static string GetSaveControllerPath(SelectableHelper target)
		{
			var defaultName = target.gameObject.name;
			var message = string.Format("Create a new animator for the game object '{0}':", defaultName);

			return EditorUtility.SaveFilePanelInProject("New Animation Contoller", defaultName, "controller", message);
		}

		static AnimationClip GenerateTriggerableTransition(string name, UnityEditor.Animations.AnimatorController controller)
		{
			var clip = UnityEditor.Animations.AnimatorController.AllocateAnimatorClip(name);
			AssetDatabase.AddObjectToAsset(clip, controller);


            var state = controller.AddMotion(clip);

            controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

            var stateMachine = controller.layers[0].stateMachine;
            var transition = stateMachine.AddAnyStateTransition(state);
            transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, name);

   //         var state = UnityEditor.Animations.AnimatorController.AddAnimationClipToController(controller, clip);

			//controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

			//var stateMachine = controller.GetLayer(0).stateMachine;
			//var transition = stateMachine.AddAnyStateTransition(state);
			//var condition = transition.GetCondition(0);
			//condition.mode = TransitionConditionMode.If;
			//condition.parameter = name;
//#endif

			return clip;
		}
	}
}