import argparse
import srt
from datetime import timedelta
from moviepy import (
    ImageClip, AudioFileClip, TextClip, CompositeVideoClip, ColorClip
)

def parse_srt_file(path):
    with open(path, 'r', encoding='utf-8') as f:
        srt_content = f.read()
    subtitles = list(srt.parse(srt_content))
    result = []
    for i, sub in enumerate(subtitles):
        print(f"{i+1}: {sub.start} --> {sub.end}: {sub.content}")
        start = sub.start.total_seconds()
        end = sub.end.total_seconds()
        text = sub.content.replace('\n', ' ')
        result.append((start, end, text))
    return result

def create_subtitle_clips(subtitles, video_size):
    subtitle_clips = []
    for start, end, text in subtitles:
        txt_clip = TextClip(
            text=text,
            font_size=32,
            color='white',
            method='caption',
            size=(int(video_size[0] * 0.8 + 20), None),
             horizontal_align='center',
            vertical_align='center',
        ).with_start(start).with_duration(end - start)

        # Extract the size of the text clip
        txt_w, txt_h = txt_clip.size
        
        # Add some padding to background to ensure no clipping
        # padding_x = 40  # horizontal padding in px
        # padding_y = 20  # vertical padding in px
        
        padding_x = 0  # horizontal padding in px
        padding_y = 0  # vertical padding in px

        bg_clip = ColorClip(
            size=(txt_w + padding_x, txt_h + padding_y),
            color=(0, 0, 0)
        ).with_opacity(0.6).with_start(start).with_duration(end - start)

        # Position the text centered inside background, with vertical offset to balance padding
        txt_clip = txt_clip.with_position((
            "center",
            padding_y // 2
        ))

        # Composite background and text, then position composite at bottom center of video
        composite = CompositeVideoClip([bg_clip, txt_clip]).with_position((
            "center",
            video_size[1] - (txt_h + padding_y + 50)  # 50 px margin from bottom
        ))

        subtitle_clips.append(composite)

    return subtitle_clips

def create_video(image_path, srt_path, audio_path, output_path):
    # Load audio and get duration
    audio = AudioFileClip(audio_path)

    # Create static image clip with same duration as audio
    image_clip = ImageClip(image_path, duration=audio.duration)

    # Parse subtitles
    subtitles = parse_srt_file(srt_path)

    # Create styled subtitle clips
    subtitle_clips = create_subtitle_clips(subtitles, image_clip.size)

    # Composite image and all subtitles
    final_clip = CompositeVideoClip(
        [image_clip] + subtitle_clips
    ).with_audio(audio)

    # Export the video
    final_clip.write_videofile(output_path, fps=24, codec="libx264")
    print(f"Video saved to {output_path}!")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Create a video from image, subtitles (SRT), and audio.")
    parser.add_argument("--image", required=True, help="Path to the image file")
    parser.add_argument("--srt", required=True, help="Path to the SRT subtitles file")
    parser.add_argument("--audio", required=True, help="Path to the audio file")
    parser.add_argument("--output", default="output.mp4", help="Output video path (default: output.mp4)")

    args = parser.parse_args()
    create_video(args.image, args.srt, args.audio, args.output)
