﻿using UnityEngine;
using System.Collections;

public abstract class DragPanelItemBase : MonoBehaviour {
	
	public abstract void SetData<T>(T data, params object[] args);

	public abstract void ItemCallback(params object[] args);
}
