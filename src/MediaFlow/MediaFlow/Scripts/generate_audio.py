import pyttsx3
import sys
import re
import os

def clean_text(text):
    # Remove emojis and non-ASCII characters
    text = re.sub(r'[^\x00-\x7F]+', '', text)

    # Remove URLs
    text = re.sub(r'http\S+', '', text)

    # Remove common markdown/code/symbols
    text = re.sub(r'[`*_{}\[\]()#+!<>]', '', text)

    # Remove excess whitespace
    text = ' '.join(text.split())

    # Optional: replace common abbreviations
    replacements = {
        "ASAP": "as soon as possible",
        "FYI": "for your information",
        "&": "and",
    }
    for k, v in replacements.items():
        text = text.replace(k, v)

    return text

def generate_audio(text, output_path="output_audio.wav"):
    engine = pyttsx3.init()
    
     # Optional: list voices to choose from
    voices = engine.getProperty('voices')
    
    for i, voice in enumerate(voices):
        print(f"Voice {i}:")
        print(f" - ID: {voice.id}")
        print(f" - Name: {voice.name}")
        print(f" - Language(s): {voice.languages}")
        print(f" - Gender: {voice.gender}")
        print(f" - Age: {voice.age}")
        print()

    engine.setProperty('voice', voices[0].id)  # Choose voice by index or matching voice.id

    # Set speaking rate (lower = slower, higher = faster)
    engine.setProperty('rate', 180)

    engine.save_to_file(text, output_path)
    engine.runAndWait()
    print(f"[OK] Audio saved to: {output_path}")

def main():
    if len(sys.argv) < 2:
        print("Usage: python generate_audio.py <text_file_path> [<output_audio_path>]")
        sys.exit(1)

    text_file_path = sys.argv[1]
    output_audio_path = sys.argv[2] if len(sys.argv) > 2 else "output_audio.wav"

    if not os.path.isfile(text_file_path):
        print(f"[!] File not found: {text_file_path}")
        sys.exit(1)

    with open(text_file_path, "r", encoding="utf-8") as f:
        raw_text = f.read()

    cleaned_text = clean_text(raw_text)
    generate_audio(cleaned_text, output_audio_path)

if __name__ == "__main__":
    main()
