import argparse
from moviepy import ImageClip, AudioFileClip, CompositeVideoClip, TextClip
from moviepy.video.fx import Resize

from moviepy import *

def create_video(image_path, text, audio_path, output_path, duration=None):
    # Load audio
    audio = AudioFileClip(audio_path)
    duration = duration or audio.duration

    # Load and animate image
    image_clip = (
        ImageClip(image_path)
        #.resized(width=1280)
        .with_duration(duration)
        # .with_effects(Resize, )  # HD width
        .with_audio(audio)
    )

    # Text overlay
    text_clip = (
        TextClip(
            text=text,
            font_size=48,
            # font='Arial-Bold',
            color='white',
            stroke_color='black',
            stroke_width=2,
            method='caption',
            size=(1000, None)
        )
        .with_duration(duration)
        .with_position(('center', 'bottom'))
    )

    # Combine video and text
    final = CompositeVideoClip([image_clip, text_clip])
    final.write_videofile(output_path, fps=24, codec='libx264', audio_codec='aac')
    print(f"Video saved to: {output_path}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--image", required=True, help="Path to background image")
    parser.add_argument("--text", required=True, help="Text to display on video")
    parser.add_argument("--audio", required=True, help="Path to narration WAV file")
    parser.add_argument("--output", required=True, help="Output video path (MP4)")
    args = parser.parse_args()

    create_video(args.image, args.text, args.audio, args.output)
