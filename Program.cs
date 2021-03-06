﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using personal.Hubs;

namespace tennis1
{
    public class Program
    {
        //common shared objects
        public static IHubContext<tennisHub> GlobalHubContext;
        public static List<TennisGame> SharedObj = new List<TennisGame>();

        public static void addAndRunNewTennisGame(TennisGame newTennisGame)
        {
            SharedObj.Add(newTennisGame);  
            ThreadStart gamePlaying = new ThreadStart(newTennisGame.start);
            Thread gameThread = new Thread(gamePlaying);
            gameThread.Start();
        }

        public static void Main(string[] args)
        {
             // BuildWebHost(args).Run();
            ThreadStart webPage = new ThreadStart(BuildWebHost(args).Run);
            Thread webThread = new Thread(webPage);
            webThread.Start();

            //Instanciar del webPage un IHubContext<progressHub> hubContext
            addAndRunNewTennisGame(new TennisGame(0, 650,400,640/2, 480/2,1.0f,1.0f, false));
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
