# main.py
import subprocess
import argparse
from pathlib import Path
import uuid

def run_command(cmd):
    result = subprocess.run(cmd, shell=True)
    if result.returncode != 0:
        raise RuntimeError(f"Command failed: {cmd}")

def orchestrate(image, text, out_video):
    temp_audio = f"temp_{uuid.uuid4().hex}.wav"

    # Step 1: Generate audio
    run_command(f"python generate_audio.py --text \"{text}\" --output \"{temp_audio}\"")

    # Step 2: Generate video
    run_command(f"python generate_video.py --image \"{image}\" --text \"{text}\" --audio \"{temp_audio}\" --output \"{out_video}\"")

    # Optional: Cleanup
    Path(temp_audio).unlink(missing_ok=True)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--image", required=True)
    parser.add_argument("--text", required=True)
    parser.add_argument("--output", required=True)
    args = parser.parse_args()

    orchestrate(args.image, args.text, args.output)
