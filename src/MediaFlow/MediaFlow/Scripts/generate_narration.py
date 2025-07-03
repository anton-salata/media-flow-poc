import logging

logging.basicConfig(
    level=logging.DEBUG,   # or logging.INFO for less verbose
    format='%(asctime)s %(levelname)s %(name)s: %(message)s'
)

import argparse
import asyncio
import edge_tts
import os


async def list_voices():
    print("\n[Available Voices]")
    voices = await edge_tts.list_voices()
    for voice in voices:
        print(f"- {voice['ShortName']} | {voice['Locale']} | {voice['Gender']}")
    print("\n---\n")
    return voices


async def generate_narration(text_file: str, output_file: str, voice: str):
    if not os.path.isfile(text_file):
        print(f"[ERROR] File not found: {text_file}")
        return

    with open(text_file, "r", encoding="utf-8") as f:
        text = f.read().strip()

    if not text:
        print("[ERROR] Text file is empty.")
        return

    print(f"[INFO] Generating narration with voice: {voice}")
    # communicate = edge_tts.Communicate(
    #     text=text,
    #     voice=voice,
    #     rate="+0%",     # Change speed here
    #     volume="+0dB",  # Change volume here
    #     pitch="+0Hz"    # Change pitch here
    # )

    communicate = edge_tts.Communicate(
        text=text,
        voice=voice,
        rate="+0%",     # speed, can be e.g. "-10%", "+20%"
        volume="+0%",   # volume, e.g. "-10%", "+10%"
        pitch="+0Hz"    # pitch, must be in Hz with +/- (usually works)
    )

    await communicate.save(output_file)
    print(f"[OK] Narration saved to: {output_file}")


async def main():
    parser = argparse.ArgumentParser(description="Generate narration using Edge-TTS")
    parser.add_argument("--text_file", help="Path to input text file")
    parser.add_argument("--output_file", help="Path to output audio file")
    parser.add_argument("--voice", default="en-US-AriaNeural", help="Voice to use (default: en-US-AriaNeural)")
    args = parser.parse_args()

    #await list_voices()
    await generate_narration(args.text_file, args.output_file, args.voice)


if __name__ == "__main__":
    asyncio.run(main())
