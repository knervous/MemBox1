using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace MemoryBox
{
    public class MemoryModel
    {

        private string name = "";
        private string owner = "";
        private List<string> textMemTitle = new List<string>();
        private List<string> textMemText = new List<string>();
        private List<string> picMemTitle = new List<string>();
        private List<string> picMemUrl = new List<string>();
        private List<string> picCreatedBy = new List<string>();
        private List<string> textCreatedBy = new List<string>();


        public MemoryModel()
        {
            textMemTitle.Add("A Tale of Two Cities");
            textMemText.Add("It was the best of times, it was the worst of times, it was the age of wisdom, it was the age of foolishness, it was the epoch of belief, it was the epoch of incredulity, it was the season of Light, it was the season of Darkness, it was the spring of hope, it was the winter of despair, we had everything before us, we had nothing before us, we were all going direct to Heaven, we were all going direct the other way--in short, the period was so far like the present period, that some of its noisiest authorities insisted on its being received, for good or for evil, in the superlative degree of comparison only");
            picMemTitle.Add("Here's a cat");
            picMemUrl.Add("http://i.imgur.com/ZhMNg8p.jpg");
            textMemTitle.Add("TEST TITLE");
            textMemText.Add("TEST TEXT");
            picCreatedBy.Add("Paul Johnson");
            textCreatedBy.Add("Paul Johnson");
            textCreatedBy.Add("Paul Johnson");
        }

        public List<string> PicCreatedBy
        {
            get
            {
                return picCreatedBy;
            }
            set
            {
                picCreatedBy = value;
            }
        }

        public List<string> TextCreatedBy
        {
            get
            {
                return textCreatedBy;
            }
            set
            {
                textCreatedBy = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }

        public List<string> TextMemTitle
        {
            get
            {
                return textMemTitle;
            }

            set
            {
                textMemTitle = value;
            }
        }

        public List<string> TextMemText
        {
            get
            {
                return textMemText;
            }

            set
            {
                textMemText = value;
            }
        }

        public List<string> PicMemTitle
        {
            get
            {
                return picMemTitle;
            }

            set
            {
                picMemTitle = value;
            }
        }

        public List<string> PicMemUrl
        {
            get
            {
                return picMemUrl;
            }

            set
            {
                picMemUrl = value;
            }
        }
    }
}