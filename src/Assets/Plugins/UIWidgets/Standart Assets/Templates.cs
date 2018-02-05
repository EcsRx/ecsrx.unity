using UnityEngine;
using System.Collections.Generic;
using System;

namespace UIWidgets {

	/// <summary>
	/// Templates for UI.
	/// </summary>
	public class Templates<T> where T : MonoBehaviour, ITemplatable {
		Dictionary<string,T> templates = new Dictionary<string,T>();
		
		Dictionary<string,Stack<T>> cache = new Dictionary<string,Stack<T>>();

		bool findTemplatesCalled;
		Action<T> onCreateCallback;

		/// <summary>
		/// Initializes a new instance of the UIWidgets.Templates class.
		/// </summary>
		/// <param name="onCreateCallback">On create new UI object callback.</param>
		public Templates(Action<T> onCreateCallback = null)
		{
			this.onCreateCallback = onCreateCallback;
		}

		/// <summary>
		/// Finds the templates.
		/// </summary>
		public void FindTemplates()
		{
			findTemplatesCalled = true;

			Resources.FindObjectsOfTypeAll<T>().ForEach(AddTemplate);
		}

		void AddTemplate(T template)
		{
			Add(template.name, template, replace: true);
			template.gameObject.SetActive(false);
		}

		/// <summary>
		/// Clears the cached instance of templates.
		/// </summary>
		public void ClearCache()
		{
			cache.Keys.ForEach(ClearCache);
		}

		/// <summary>
		/// Clears the cached instance of specified template.
		/// </summary>
		/// <param name="name">Template name.</param>
		public void ClearCache(string name)
		{
			if (!cache.ContainsKey(name))
			{
				return ;
			}
			cache[name].ForEach(DestroyCached);
			cache[name].Clear();
			cache[name].TrimExcess();
		}

		void DestroyCached(T cached)
		{
			if ((cached!=null) && ((cached.gameObject!=null)))
			{
				UnityEngine.Object.Destroy(cached.gameObject);
			}
		}

		/// <summary>
		/// Check if exists template with the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public bool Exists(string name)
		{
			return templates.ContainsKey(name);
		}

		/// <summary>
		/// Gets the template by name.
		/// </summary>
		/// <returns>The template.</returns>
		/// <param name="name">Template name.</param>
		public T Get(string name)
		{
			if (!Exists(name))
			{
				throw new ArgumentException("Not found template with name '" + name + "'");
			}
			
			return templates[name];
		}

		/// <summary>
		/// Deletes the template by name.
		/// </summary>
		/// <param name="name">Template name.</param>
		public void Delete(string name)
		{
			if (!Exists(name))
			{
				return ;
			}
			
			templates.Remove(name);
			ClearCache(name);
		}

		/// <summary>
		/// Adds the template.
		/// </summary>
		/// <param name="name">Template name.</param>
		/// <param name="template">Template object.</param>
		/// <param name="replace">If set to <c>true</c> replace.</param>
		public void Add(string name, T template, bool replace = true)
		{
			if (Exists(name))
			{
				if (!replace)
				{
					throw new ArgumentException("Template with name '" + name + "' already exists.");
				}
				
				ClearCache(name);
				templates[name] = template;
			}
			else
			{
				templates.Add(name, template);
			}
			template.IsTemplate = true;
			template.TemplateName = name;
		}

		/// <summary>
		/// Return instance by the specified template name.
		/// </summary>
		/// <param name="name">Template name.</param>
		public T Instance(string name)
		{
			if (!findTemplatesCalled)
			{
				FindTemplates();
			}

			if (!Exists(name))
			{
				throw new ArgumentException("Not found template with name '" + name + "'");
			}
			if (templates[name]==null)
			{
				templates.Clear();
				FindTemplates();
			}
			
			T template;
			if ((cache.ContainsKey(name)) && (cache[name].Count > 0))
			{
				template = cache[name].Pop();
			}
			else
			{
				template = UnityEngine.Object.Instantiate(templates[name]) as T;

				template.TemplateName = name;
				template.IsTemplate = false;

				if (onCreateCallback!=null)
				{
					onCreateCallback(template);
				}
			}
			template.transform.SetParent(templates[name].transform.parent, false);

			return template;
		}

		/// <summary>
		/// Returns instance to the cache.
		/// </summary>
		/// <param name="instance">Instance</param>
		public void ToCache(T instance)
		{
			instance.gameObject.SetActive(false);

			if (!cache.ContainsKey(instance.TemplateName))
			{
				cache[instance.TemplateName] = new Stack<T>();
			}
			cache[instance.TemplateName].Push(instance);
		}
	}
}