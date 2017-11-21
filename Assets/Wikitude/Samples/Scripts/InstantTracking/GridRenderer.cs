using UnityEngine;

public class GridRenderer : MonoBehaviour 
{
	private static Color TargetColor = new Color(1.0f, 0.525f, 0, 1.0f);
	private static Color GridColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	private static Color MainLineColor = GridColor * 0.9f;
	private static Color UnitLineColor = GridColor * 0.75f;

	private const int NumberOfLinesOnEitherSide = 50;
	private const float LineSpacing = 0.25f;
	private const int IntervalBetweenMainLines = 10;
	private const float TargetSize = 8.0f; 
	
	private Material _lineMaterial;

	private void Start() {
		InitializeLineMaterial();
	}

	private void OnRenderObject() {
		// Because the Wikitude SDK uses a secondary camera to render the camera frame, we need this check here.
		if (Camera.current == Camera.main) {
			RenderGrid();
		}
	}

	private void InitializeLineMaterial() {
		var shader = Shader.Find("Hidden/Internal-Colored");
		_lineMaterial = new Material(shader);
		_lineMaterial.hideFlags = HideFlags.HideAndDontSave;

		_lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		_lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

		_lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		_lineMaterial.SetInt("_ZWrite", 0);
	}

	private void RenderGrid() {
		_lineMaterial.SetPass(0);
		GL.Begin(GL.LINES);

		RenderGrid(Vector3.forward, Vector3.right);
		RenderGrid(Vector3.right, Vector3.forward);

		RenderLine(Vector3.forward,  Vector3.zero, TargetColor, TargetSize);
		RenderLine(Vector3.right,  Vector3.zero, TargetColor, TargetSize);

		GL.End();
	}

	private void RenderGrid(Vector3 mainAxis, Vector3 perpendicular) {

		for (int i = -NumberOfLinesOnEitherSide; i <= NumberOfLinesOnEitherSide; ++i) {
			var color = UnitLineColor;
			if (i == 0) {
				color = GridColor;
			} else if (i % IntervalBetweenMainLines == 0) {
				color = MainLineColor;
			}
			float intensity = 1.0f - Mathf.Abs(i * 2.0f) / NumberOfLinesOnEitherSide;
			color.a *= intensity;
			if (color.a > 0.01f) {
				RenderLine(mainAxis, perpendicular * i * LineSpacing, color, (float)NumberOfLinesOnEitherSide / 8.0f);
			}
		}
	}

	private void RenderLine(Vector3 axis, Vector3 offset, Color color, float length) {
		GL.Color(GetTransparent(color));
		GL.Vertex(axis * (-length) + offset);
		GL.Color(color);
		GL.Vertex(offset);
		GL.Vertex(offset);
		GL.Color(GetTransparent(color));
		GL.Vertex(axis * length + offset);
	}

	private Color GetTransparent(Color color) {
		color.a = 0.0f;
		return color;
	}
}
