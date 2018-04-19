using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cocosocket4unity;
using System;
using protocol;
using System.Reflection;
using Game;

public class MessageQueueHandler : MonoBehaviour {
	static Queue<QueueItem> messageQueue;
	static Queue<QueueItem> errorQueue;
	static Dictionary<short, Type> typeMapping;
	static Dictionary<Type, short> cmdMapping;
	// Use this for initialization
	void Awake () {
		messageQueue = new Queue<QueueItem>();
		errorQueue = new Queue<QueueItem>();
		typeMapping = new Dictionary<short, Type>();
		cmdMapping = new Dictionary<Type, short>();
		List<Type> ls = ClassUtil.GetClasses("protocol");
		foreach (Type item in ls) {
			ProtoAttribute arr = (ProtoAttribute)ClassUtil.GetAttribute(item,typeof(ProtoAttribute));
			if(arr != null) {
				typeMapping.Add((short)arr.value, item);
				cmdMapping.Add(item, (short)arr.value);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (messageQueue.Count > 0) {
			lock(messageQueue) {
				QueueItem queueItem = messageQueue.Dequeue();
				DoCallback(queueItem.Model);
			}
		}
		else if (errorQueue.Count > 0) {
			lock(errorQueue) {
				QueueItem errorItem = errorQueue.Dequeue();
			}
		}
	}

	public static void PushQueue(short cmd, object param) {
		lock(messageQueue) {
			messageQueue.Enqueue(new QueueItem(cmd, param));
		}
	}

	/// <summary>
	/// 推送错误消息
	/// </summary>
	/// <param name="msg">Message.</param>
	/// <param name="state">State.</param>
	public static void PushError(string msg, short state = 0) {
		lock(errorQueue) {
			errorQueue.Enqueue(new QueueItem(state, "", msg));
		}
	}

	/// <summary>
	/// 获取协议号对应的实体
	/// </summary>
	/// <returns>The protocol type.</returns>
	/// <param name="cmd">Cmd.</param>
	public static Type GetProtocolType(short cmd) {
		if (typeMapping.ContainsKey(cmd)) {
			return typeMapping[cmd];
		}
		return null;
	}

	/// <summary>
	/// 获取实体类对应的协议号对应
	/// </summary>
	/// <returns>The protocol CM.</returns>
	/// <param name="type">Type.</param>
	public static short GetProtocolCMD(Type type) {
		if (cmdMapping.ContainsKey(type)) {
			return cmdMapping[type];
		}
		return 0;
	}

	/// <summary>
	/// 回调方法执行
	/// </summary>
	/// <param name="param">Parameter.</param>
	private static void DoCallback(object param) 
    {
        Messenger.Broadcast(param);
	}
}
