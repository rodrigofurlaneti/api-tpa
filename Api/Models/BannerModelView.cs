﻿using System;

namespace Api.Models
{
    public class BannerModelView
    {
        public BannerModelView()
        {
            
        }

        public BannerModelView(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL não foi preenchida.");

            URL = url;
        }

        public string URL { get; set; }
    }
}