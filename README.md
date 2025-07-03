# media-flow-poc
🧪 [POC] Media Flow:  Facts Videos - AI-powered pipeline that transforms "Did You Know?" facts into shorts videos. (C#+Python interop)</br> Note: New improved version in development

⚠️ This repository contains experimental code for a POC. A redesigned version with enhanced capabilities is currently in development.

---

Tech Stack
</br>
• C# (Web scraping, orchestration)
</br>
• Python and REST API (AI/ML services)
</br>
• C#<->Python interop
– C# ↔ Python via REST APIs and subprocesses

---

## 📚 Libraries Used

### 🧠 AI & Machine Learning

- [OpenAI API](https://platform.openai.com/docs) – Chat and completion generation
- [OpenRouter](https://openrouter.ai/) – Unified API router for multiple AI models
- [FusionBrain](https://fusionbrain.ai/) – AI-powered image/text content generation
- [Whisper](https://github.com/openai/whisper) – Automatic speech recognition (ASR)
- [TTS by coqui-ai](https://github.com/coqui-ai/TTS) – Neural Text-to-Speech
- [edge-tts](https://github.com/rany2/edge-tts) – Microsoft Edge text-to-speech
- [aeneas](https://github.com/readbeyond/aeneas) – Audio and subtitle alignment
- [MusicGen (Audiocraft)](https://github.com/facebookresearch/audiocraft) – Music generation from text prompts

### 🎬 Video & Audio Processing

- [moviepy](https://github.com/Zulko/moviepy) – Video editing and compositing
- [torchaudio](https://pytorch.org/audio/stable/index.html) – Audio manipulation (based on PyTorch)
- [pyttsx3](https://pyttsx3.readthedocs.io/) – Offline text-to-speech (Python)
- [pysubs2](https://github.com/tkarabela/pysubs2) – Subtitle rendering and styling
- [opencv-python (cv2)](https://pypi.org/project/opencv-python/) – Image and video frame processing
- [Pillow (PIL)](https://python-pillow.org/) – Image processing and manipulation

### ⏱ Subtitle Sync & Formatting

- [srt](https://github.com/cdown/srt) – Subtitle parsing and manipulation
- [pysubs2](https://github.com/tkarabela/pysubs2) – Advanced subtitle editing and timing

### 📺 YouTube & Google API (C#)

- [Google.Apis.YouTube.v3](https://developers.google.com/youtube/registering_an_application) – YouTube Data API
- [Google.Apis.Auth.OAuth2](https://developers.google.com/identity/protocols/oauth2) – OAuth2 Authentication
- [Google.Apis.Services](https://github.com/googleapis/google-api-dotnet-client) – API base classes
- [Google.Apis.Upload](https://github.com/googleapis/google-api-dotnet-client) – Upload management

### 🌐 Web Scraping & HTML Processing

- [HtmlAgilityPack](https://html-agility-pack.net/) – HTML parsing and scraping (C#)

### 🔧 Utilities & Support

- [subprocess](https://docs.python.org/3/library/subprocess.html) – Shell process execution
- [argparse](https://docs.python.org/3/library/argparse.html) – Command-line argument parsing
- [asyncio](https://docs.python.org/3/library/asyncio.html) – Asynchronous programming
- [numpy](https://numpy.org/) – Numerical computations and image processing
- [collections.Counter](https://docs.python.org/3/library/collections.html#collections.Counter) – Word and element frequency tracking
- [datetime.timedelta](https://docs.python.org/3/library/datetime.html#timedelta) – Duration and timing operations

---

Stay tuned for the upcoming version with a cleaner pipeline, better audio quality, and full integration for dynamic content rendering!
