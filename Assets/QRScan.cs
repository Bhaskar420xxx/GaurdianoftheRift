using UnityEngine;
using ZXing;

public class QRScan : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToInstantiate; // Add a reference to the prefab you want to instantiate

    private WebCamTexture camTexture;
    private Color32[] cameraColorData;
    private int width, height;

    private IBarcodeReader barcodeReader = new BarcodeReader
    {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions
        {
            TryHarder = false,
            PossibleFormats = new System.Collections.Generic.List<BarcodeFormat> { BarcodeFormat.QR_CODE }
        }
    };

    private Result result;

    private void Start()
    {
        SetupWebcamTexture();
        PlayWebcamTexture();
        cameraColorData = new Color32[width * height];
    }

    private void OnEnable()
    {
        PlayWebcamTexture();
    }

    private void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Pause();
        }
    }

    private void Update()
    {
        camTexture.GetPixels32(cameraColorData);
        result = barcodeReader.Decode(cameraColorData, width, height);

        if (result != null)
        {
            // Instantiate the prefab at the position where the QR code is scanned
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2.0f; // Adjust the distance as needed
            Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        camTexture.Stop();
    }

    private void SetupWebcamTexture()
    {
        camTexture = new WebCamTexture(1920, 1080, 30);
    }

    private void PlayWebcamTexture()
    {
        if (camTexture != null)
        {
            camTexture.Play();
            width = camTexture.width;
            height = camTexture.height;
        }
    }
}
