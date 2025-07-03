# media-flow-poc
🧪 [POC] Media Flow:  Facts Videos - AI-powered pipeline that transforms "Did You Know?" facts into shorts videos. (C#+Python interop)</br> Note: New improved version in development

⚠️ This repository contains experimental code for a POC. A redesigned version with enhanced capabilities is currently in development.

This AI-powered pipeline demonstrates automated content creation

Tech Stack
</br>
• C# (Web scraping, orchestration)
</br>
• Python and REST API (AI/ML services)
</br>
• C#<->Python interop


## Libraries Used

### 🧠 AI & ML
- [OpenAI API](https://platform.openai.com/docs) – Used for chat/completion generation
- [OpenRouter](https://openrouter.ai/) – Unified API router for AI models
- [FusionBrain](https://fusionbrain.ai/) – AI-powered image/text tools
- [Whisper](https://github.com/openai/whisper) – Speech recognition
- [TTS by coqui-ai](https://github.com/coqui-ai/TTS) – Neural Text-to-Speech
- [edge-tts](https://github.com/rany2/edge-tts) – Microsoft Edge text-to-speech API
- [aeneas](https://github.com/readbeyond/aeneas) – Automatic audio-text synchronization
- [MusicGen (audiocraft)](https://github.com/facebookresearch/audiocraft) – Audio generation (Facebook's MusicGen)

### 📺 Video & Audio Processing
- [moviepy](https://github.com/Zulko/moviepy) – Video editing and compositing
- [torchaudio](https://pytorch.org/audio/stable/index.html) – Audio processing (PyTorch)
- [pyttsx3](https://pyttsx3.readthedocs.io/) – Offline TTS engine for Python
- [pysubs2](https://github.com/tkarabela/pysubs2) – Subtitle handling
- [opencv-python (cv2)](https://pypi.org/project/opencv-python/) – Computer vision processing
- [Pillow (PIL)](https://python-pillow.org/) – Image processing

### ⏱ Subtitles & Sync
- [srt](https://github.com/cdown/srt) – Subtitle parsing and manipulation
- [pysubs2](https://github.com/tkarabela/pysubs2) – Advanced subtitle editing and styling

### 📺 YouTube & Google APIs
- [Google.Apis.YouTube.v3](https://developers.google.com/youtube/registering_an_application) – YouTube Data API for C#
- [Google.Apis.Auth.OAuth2](https://developers.google.com/identity/protocols/oauth2) – Google OAuth2 authentication
- [Google.Apis.Services](https://github.com/googleapis/google-api-dotnet-client) – Base client services for Google APIs
- [Google.Apis.Upload](https://github.com/googleapis/google-api-dotnet-client) – Upload management

### 🌐 Web & HTML
- [HtmlAgilityPack](https://html-agility-pack.net/) – HTML parsing and scraping (C#)

### 🔧 Utilities
- [subprocess](https://docs.python.org/3/library/subprocess.html) – Run shell commands
- [argparse](https://docs.python.org/3/library/argparse.html) – CLI argument parsing
- [asyncio](https://docs.python.org/3/library/asyncio.html) – Async I/O in Python
- [numpy](https://numpy.org/) – Numerical computing
- [collections.Counter](https://docs.python.org/3/library/collections.html#collections.Counter) – Frequency counting
- [datetime.timedelta](https://docs.python.org/3/library/datetime.html#timedelta) – Time duration
