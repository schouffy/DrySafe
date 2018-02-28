# DrySafe
Azure function that gets weather forecast for the next hour and sends a notification if any rain is planned

## What is it ?
I bike to my workplace. Just before i leave home or work, this Azure function runs and :
- Checks if there is rain to come in the upcoming hour (i use weather forecast from [meteofrance.com](meteofrance.com) but i can recommand [https://darksky.net/dev/docs](DarkSky API), using [https://github.com/jcheng31/DarkSkyApi](this library) for C#)
- It will then send a notification to my phone describing the expected rain (when, how strong,..) using [https://www.pushbullet.com/](PushBullet) and [https://github.com/adamyeager/PushbulletSharp](this library).
