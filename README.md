# DrySafe
Azure function that gets weather forecast for the next hour and sends a notification if any rain is planned

## What is it ?
I bike to my workplace. Just before i leave home or work, this Azure function runs and :
- Checks if there is rain to come in the upcoming hour (i use weather forecast from [MeteoFrance](https://meteo.fr) but i can recommand [DarkSky API](https://darksky.net/dev/docs), using [this library](https://github.com/jcheng31/DarkSkyApi) for C#)
- It will then send a notification to my phone describing the expected rain (when, how strong,..) using [PushBullet](https://www.pushbullet.com/) and [this library](https://github.com/adamyeager/PushbulletSharp).
