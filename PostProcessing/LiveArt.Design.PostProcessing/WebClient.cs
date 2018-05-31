namespace LiveArt.Design.PostProcessing
{
    internal interface IWebClientWrapper //required for testing
    {
         void DownloadFile(string address,string filename);
    }


    internal class WebClientWrapper:IWebClientWrapper{

        

        public void DownloadFile(string address, string filename)
        {

            using(var realClient=new System.Net.WebClient()){
               realClient.DownloadFile(address,filename);    
            }

        }
    }




}
