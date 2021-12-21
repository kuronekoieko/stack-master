using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    #region position

    public static void SetPosX(this RectTransform self, float x)
    {
        self.position = new Vector3(x, self.position.y, self.position.z);
    }

    public static void SetPosY(this RectTransform self, float y)
    {
        self.position = new Vector3(self.position.x, y, self.position.z);
    }

    public static void SetPosZ(this RectTransform self, float z)
    {
        self.position = new Vector3(self.position.x, self.position.y, z);
    }

    public static void AddPosX(this RectTransform self, float x)
    {
        self.position = new Vector3(self.position.x + x, self.position.y, self.position.z);
    }

    public static void AddPosY(this RectTransform self, float y)
    {
        self.position = new Vector3(self.position.x, self.position.y + y, self.position.z);
    }

    public static void AddPosZ(this RectTransform self, float z)
    {
        self.position = new Vector3(self.position.x, self.position.y, self.position.z + z);
    }

    #endregion

    #region anchoredPosition

    public static void SetAnchoredPositionX(this RectTransform self, float x)
    {
        self.anchoredPosition = new Vector2(x, self.anchoredPosition.y);
    }

    public static void SetAnchoredPositionY(this RectTransform self, float y)
    {
        self.anchoredPosition = new Vector2(self.anchoredPosition.x, y);
    }

    public static void AddAnchoredPositionX(this RectTransform self, float x)
    {
        self.anchoredPosition = new Vector2(self.anchoredPosition.x + x, self.anchoredPosition.y);
    }

    public static void AddAnchoredPositionY(this RectTransform self, float y)
    {
        self.anchoredPosition = new Vector2(self.anchoredPosition.x, self.anchoredPosition.y + y);
    }

    #endregion

    #region anchoredPosition3D

    public static void SetAnchoredPosition3DX(this RectTransform self, float x)
    {
        self.anchoredPosition3D = new Vector3(x, self.anchoredPosition3D.y, self.anchoredPosition3D.z);
    }

    public static void SetAnchoredPosition3DY(this RectTransform self, float y)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, y, self.anchoredPosition3D.z);
    }

    public static void SetAnchoredPosition3DZ(this RectTransform self, float z)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, self.anchoredPosition3D.y, z);
    }

    public static void AddAnchoredPosition3DX(this RectTransform self, float x)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x + x, self.anchoredPosition3D.y, self.anchoredPosition3D.z);
    }

    public static void AddAnchoredPosition3DY(this RectTransform self, float y)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, self.anchoredPosition3D.y + y, self.anchoredPosition3D.z);
    }

    public static void AddAnchoredPosition3DZ(this RectTransform self, float z)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, self.anchoredPosition3D.y, self.anchoredPosition3D.z + z);
    }

    #endregion
}
