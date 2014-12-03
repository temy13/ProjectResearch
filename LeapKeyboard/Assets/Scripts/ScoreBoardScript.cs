using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
using Leap;
using Aspose.Pdf;
using Aspose.Pdf.Devices;


public class ScoreBoardScript : MonoBehaviour {

	public string debug;
	public GameObject scoreboard;
	//score
	byte[][] image_bytes = new byte[4][];
	int scorenumber;
	//swipe
	Controller controller = new Controller();
	// Use this for initialization
	void Start () {

		//SetScore (LoadBin(Application.dataPath + "/6612.jpg"));
		//SetScoreFromPDF ("melt.pdf");
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();

		for(int g = 0; g < frame.Gestures().Count; g++){
			Gesture gesture = frame.Gestures()[g];
			debug += frame.Gestures()[g].ToString();
			if(gesture.Type == Gesture.GestureType.TYPE_SWIPE) {
				ChangeScore(new SwipeGesture(gesture));
			}
		}
	}
	//image file to score
	void SetScore(byte[] score_bytes){
		Texture2D tex = new Texture2D(0,0);
		tex.LoadImage(score_bytes);
		scoreboard.renderer.material.mainTexture = tex;
	}

	public void SetScoreFromImg(string imgfile){
		string file = Application.dataPath + "/" + imgfile;
		SetScore (LoadBin(file));
		}

	public void SetScoreFromPDF(string pdffile){
		string[] imagePaths = ImageFromPDF.ConvertPDFtoImg (Application.dataPath + "/"+pdffile);
		for (int i = 0; i<imagePaths.Length; i++) {
			image_bytes [i] = LoadBin (imagePaths [i]);
		}
		//tex.LoadImage (LoadBin(imagePaths[0]));
		scorenumber = 0;
		SetScore (image_bytes [scorenumber]);
		}
	

	byte[] LoadBin(string path){
		FileStream fs = new FileStream(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fs);
		byte[] buf = br.ReadBytes( (int)br.BaseStream.Length);
		br.Close();
		return buf;
	}

	void ChangeScore(SwipeGesture swipe){
		if (swipe.StartPosition.x < swipe.Position.x) {
			if(image_bytes[scorenumber] != null){
				scorenumber++;
				SetScore (image_bytes [scorenumber]);
				}
		}else if (swipe.StartPosition.x > swipe.Position.x) {
			if(scorenumber > 0)
				scorenumber--;
				SetScore (image_bytes [scorenumber]);
				}
		}
}

public class ImageFromPDF
{

	public static string[] ConvertPDFtoImg(string pdffile){

		string[] imagePath = new string[4];

       /* Document pdfDocument = new Document(pdffile);
		
		for (int pageCount = 1; pageCount <= pdfDocument.Pages.Count 
		     && pageCount <= 4; pageCount++)
		{
			using (FileStream imageStream = new FileStream(Application.dataPath +"image" + pageCount + ".jpg", FileMode.Create))
			{
				// Create Resolution object
				Aspose.Pdf.Devices.Resolution resolution = new Aspose.Pdf.Devices.Resolution(300);
				// Create JPEG device with specified attributes (Width, Height, Resolution, Quality)
				// where Quality [0-100], 100 is Maximum
				JpegDevice jpegDevice = new JpegDevice(resolution, 100);
				
				// Convert a particular page and save the image to stream
				jpegDevice.Process(pdfDocument.Pages[pageCount], imageStream);
				// Close stream
				imageStream.Close();
				imagePath[pageCount-1] = Application.dataPath +"/image" + pageCount + ".jpg";
			}
		}*/
		return imagePath;
		
	}

	
}




/************************************/





