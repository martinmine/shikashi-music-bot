using System;
using System.Collections.Generic;
using System.Text;

namespace ShikashiBot.Services
{
    public interface IPlayable
    {
        string Url { get; set; }

        string Uri { get; }

        string Title { get; }

        string Requester { get; set; }

        string DurationString { get; }

        int Speed { get; }

        void OnPostPlay();
    }
}
