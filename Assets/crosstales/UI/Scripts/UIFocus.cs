﻿using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI
{
	/// <summary>Change the Focus on from a Window.</summary>
    public class UIFocus : MonoBehaviour
    {
		public string CanvasName = "Canvas";

        private UIWindowManager manager;
        private Image image;

		public void Start()
        {
			manager = GameObject.Find(CanvasName).GetComponent<UIWindowManager>();

            image = transform.FindChild("Panel/Header").GetComponent<Image>();
        }

        public void onPanelEnter()
        {
            manager.ChangeState(gameObject);

            Color c = image.color;
            c.a = 255;
            image.color = c;

            transform.SetAsLastSibling(); //move to the front (on parent)
            transform.SetAsFirstSibling(); //move to the back (on parent)
            transform.SetSiblingIndex(-1); //move to position, whereas 0 is the backmost, transform.parent.childCount -1 is the frontmost position 
            transform.GetSiblingIndex(); //get the position in the hierarchy (on parent)
        }
    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)