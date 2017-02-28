using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DistributedServer.Models;
using DistributedServer.App_Code;

namespace DistributedServer
{
    public partial class Admin : System.Web.UI.Page
    {
        string fileName;
        int batchSize;
        string path = @"C:\Users\Noah\Documents\visual studio 2015\Projects\DistributedServer\DistributedServer\Data";
        BatchGen gen;
        MasterDictionary master;

        protected void Page_Load(object sender, EventArgs e)
        {
            fileName = tbFileName.Text;
            if (fileName == null)
                fileName = "Batch";

            batchSize = int.Parse(tbBatchSize.Text);
            if (batchSize == 0)
                batchSize = 1000;

            master = MasterDictionary.Instance;
            gen = new BatchGen();

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                var reader = new StreamReader(FileUpload1.FileContent);
                var document = reader.ReadToEnd();
                reader.Close();
                string fileName = tbFileName.Text;
                bool success = gen.saveDocument(document, path, fileName, fileName);
                if (success)
                {
                    int batchSize = 1000;
                    var isCustomBatch = int.TryParse(tbBatchSize.Text, out batchSize);
                    if (isCustomBatch)
                        lblBatchSize.Text = string.Format("Batch Size {0}", batchSize);
                    var docList = gen.splitDocument(document, batchSize, true);

                    // update dictionary entry
                    MasterDictionary.UpdateEntry(fileName, true, 0);

                    int docCount = 1;
                    foreach (var doc in docList)
                    {
                        var name = fileName + "_" + docCount.ToString();
                        gen.saveDocument(doc, path, fileName, name);
                        docCount++;
                    }

                    lblProcess.ForeColor = System.Drawing.Color.Green;
                    lblProcess.Text = "Done: Sucess!";
                }

                else
                {
                    lblFileUpload.ForeColor = System.Drawing.Color.Red;
                    lblFileUpload.Text = "Upload Failed: File not stored to server";
                }
            }
            else
            {
                lblFileUpload.ForeColor = System.Drawing.Color.Red;
                lblFileUpload.Text = "Upload Failed: File not found";
            }
        }

        protected void tbBatchSize_TextChanged(object sender, EventArgs e)
        {
            batchSize = int.Parse(tbBatchSize.Text);
        }

        protected void tbFileName_TextChanged(object sender, EventArgs e)
        {
            fileName = tbFileName.Text;
        }
    }
}