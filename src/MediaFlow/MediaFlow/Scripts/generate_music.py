import argparse
import torchaudio
from audiocraft.models import MusicGen

def generate_music(prompt: str, output_file: str, duration: int = 15):
    print(f"Generating {duration}s of music for prompt: '{prompt}'")

    model = MusicGen.get_pretrained('facebook/musicgen-small')
    model.set_generation_params(duration=duration)

    wav = model.generate([prompt])  # List of 1 prompt
    print(f"Saving output to {output_file}...")

    torchaudio.save(output_file, wav[0].cpu(), sample_rate=32000)
    print("Done.")


def main():
    parser = argparse.ArgumentParser(description="Generate music using Audiocraft MusicGen.")
    parser.add_argument('--output', '-o', required=True, help='Path to save the generated .wav file')
    parser.add_argument('--prompt', '-p', required=True, help='Text prompt for music generation')
    parser.add_argument('--duration', '-d', type=int, default=15, help='Duration of the generated music in seconds (default: 15)')

    args = parser.parse_args()
    generate_music(args.prompt, args.output, args.duration)


if __name__ == '__main__':
    main()
