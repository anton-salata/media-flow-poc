import os
import sys
import argparse
import unicodedata
import random
from TTS.api import TTS
import sys

sys.stdout.reconfigure(encoding='utf-8')

def clean_text(text):
    # Normalize to decomposed form (NFD)
    normalized = unicodedata.normalize('NFD', text)
    # Remove combining characters
    cleaned = ''.join(ch for ch in normalized if not unicodedata.combining(ch))
    # Normalize back to composed form (optional)
    return unicodedata.normalize('NFC', cleaned)

def list_available_models():
    print("\n[Available Coqui TTS Models]\n")

    tts_manager = TTS().list_models()
    all_models = tts_manager.list_models()
    print(all_models)

    # models = TTS().list_models()
    # for model in models:
    #     print(f" - {model}")
    print("\nUse `--model` to specify one of these.\n")

def generate_speech(text_path, output_path, model_name):
    if not os.path.isfile(text_path):
        print(f"[!] File not found: {text_path}")
        sys.exit(1)

    with open(text_path, "r", encoding="utf-8") as f:
        text = f.read().strip()

    if not text:
        print("[!] Text file is empty.")
        sys.exit(1)

    text = clean_text(text)

    print(f"[INFO] Using model: {model_name}")
    tts = TTS(model_name=model_name, progress_bar=False, gpu=False)

    print("[INFO] Synthesizing speech...")
    
    # print("[Info] Generating samples for all voices")
    
    # tts = TTS(model_name="tts_models/en/vctk/vits")

    # speaker_list = tts.speakers[1:]  # skip first
    # for speaker in speaker_list:        
    #     output_file = f"output_{speaker}.wav"
    #     tts.tts_to_file(text=text, speaker=speaker, file_path=output_file)
        
    # print(f"Generated {output_file}")
    # print("[Info] Finished generating samples for all voices")
    
    speakers = ["p313", "p248", "p251", "p254", "p330", "p263", "p267", "p268", "p270", "p273", "p274"]    
    
    tts.tts_to_file(text=text, speaker=random.choice(speakers), file_path=output_path, speed=1.3) #p243, p270, p362, p274, p330
    
    print(f"[DONE] Audio saved to: {output_path}")

def main():
    parser = argparse.ArgumentParser(description="Coqui TTS Narration Generator")
    parser.add_argument("--text_file", help="Path to the input text file")
    parser.add_argument("--output", "-o", default="output.wav", help="Path to save the output audio")
    parser.add_argument("--model", "-m", default=  "tts_models/en/vctk/vits", #"tts_models/en/ljspeech/glow-tts",  #"tts_models/en/ljspeech/fast_pitch", #"tts_models/en/ljspeech/tacotron2-DDC",
                        help="TTS model to use (see list at startup)")

    args = parser.parse_args()

    list_available_models()
    generate_speech(args.text_file, args.output, args.model)

if __name__ == "__main__":
    main()
