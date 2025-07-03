import argparse
import srt
import pysubs2
import subprocess
import random
from datetime import timedelta
from moviepy import ImageClip, AudioFileClip, CompositeVideoClip, CompositeAudioClip
from moviepy.audio import fx as afx
from moviepy.video import fx as vfx
from typing import Tuple, Dict
from PIL import Image
import numpy as np
from collections import Counter
import cv2

def luminance(color: Tuple[int, int, int]) -> float:
    """Calculate relative luminance for contrast calculation"""
    def channel_lum(c):
        c = c / 255.0
        return c / 12.92 if c <= 0.03928 else ((c + 0.055) / 1.055) ** 2.4
    r, g, b = color
    return 0.2126 * channel_lum(r) + 0.7152 * channel_lum(g) + 0.0722 * channel_lum(b)

def contrast_ratio(c1: Tuple[int, int, int], c2: Tuple[int, int, int]) -> float:
    """Calculate contrast ratio between two RGB colors (WCAG formula)"""
    L1 = luminance(c1)
    L2 = luminance(c2)
    lighter = max(L1, L2)
    darker = min(L1, L2)
    return (lighter + 0.05) / (darker + 0.05)

def pick_contrasting_subtitle_style(image_path: str, top_n: int = 3) -> Dict[str, Tuple[int, int, int]]:
    # Your palette (feel free to expand)
    palette = [
        {"primary": (255, 215, 0), "outline": (139, 69, 19)},    # Gold, brown
        {"primary": (255, 140, 0), "outline": (128, 0, 0)},      # Dark orange, dark red
        {"primary": (255, 69, 0), "outline": (128, 0, 0)},       # Red-orange, dark red
        {"primary": (255, 99, 71), "outline": (139, 69, 19)},    # Tomato, brown
        {"primary": (255, 165, 79), "outline": (160, 82, 45)},   # Light orange, sienna
        {"primary": (0, 255, 255), "outline": (0, 128, 128)},    # Cyan, teal
        {"primary": (0, 255, 127), "outline": (0, 100, 0)},      # Spring green, dark green
        {"primary": (138, 43, 226), "outline": (75, 0, 130)},    # Blue violet, indigo
        {"primary": (255, 20, 147), "outline": (199, 21, 133)},  # Deep pink, medium violet red
        {"primary": (0, 191, 255), "outline": (25, 25, 112)},    # Deep sky blue, midnight blue
        {"primary": (255, 105, 180), "outline": (139, 0, 139)},  # Hot pink, dark magenta
        {"primary": (0, 128, 128), "outline": (0, 100, 100)},    # Teal, dark cyan
        {"primary": (255, 69, 0), "outline": (128, 0, 0)},       # Red-orange, dark red
        {"primary": (65, 105, 225), "outline": (0, 0, 139)},     # Royal blue, dark blue
        {"primary": (154, 205, 50), "outline": (85, 107, 47)},   # Yellow green, dark olive green
        {"primary": (0, 250, 154), "outline": (46, 139, 87)},    # Medium spring green, sea green
        {"primary": (173, 216, 230), "outline": (70, 130, 180)}, # Light blue, steel blue
        {"primary": (218, 112, 214), "outline": (148, 0, 211)},  # Orchid, dark violet
        {"primary": (244, 164, 96), "outline": (160, 82, 45)},   # Sandy brown, sienna
        {"primary": (100, 149, 237), "outline": (25, 25, 112)},  # Cornflower blue, midnight blue
        {"primary": (124, 252, 0), "outline": (85, 107, 47)},    # Lawn green, dark olive green
        {"primary": (255, 182, 193), "outline": (199, 21, 133)}, # Light pink, medium violet red
        {"primary": (186, 85, 211), "outline": (75, 0, 130)},    # Medium orchid, indigo
        {"primary": (240, 128, 128), "outline": (165, 42, 42)},  # Light coral, brown
        {"primary": (0, 206, 209), "outline": (0, 139, 139)},    # Dark turquoise, dark cyan
        {"primary": (255, 160, 122), "outline": (205, 92, 92)},  # Light salmon, Indian red
        {"primary": (144, 238, 144), "outline": (34, 139, 34)},  # Light green, forest green
        {"primary": (255, 228, 181), "outline": (210, 105, 30)}, # Moccasin, chocolate
        {"primary": (221, 160, 221), "outline": (138, 43, 226)}, # Plum, blue violet
    ]

    # Load and resize image small for speed
    img = cv2.imread(image_path)
    if img is None:
        raise ValueError(f"Could not load image at path: {image_path}")
    small_img = cv2.resize(img, (50, 50))
    avg_color = tuple(np.mean(small_img.reshape(-1, 3), axis=0).astype(int))
    avg_color = (avg_color[2], avg_color[1], avg_color[0])  # Convert BGR to RGB

   # Calculate and sort by contrast ratio
    palette_with_contrast = [
        (style, contrast_ratio(style["primary"], avg_color)) for style in palette
    ]
    palette_with_contrast.sort(key=lambda x: x[1], reverse=True)

    # Pick randomly from top N
    top_styles = [style for style, _ in palette_with_contrast[:top_n]]
    chosen_style = random.choice(top_styles)

    return chosen_style

def parse_srt_file(srt_path):
    with open(srt_path, 'r', encoding='utf-8') as f:
        content = f.read()
    subtitles = list(srt.parse(content))
    return subtitles

def convert_srt_to_ass(srt_path, ass_path, image_path):
    print("Converting SRT to ASS...")
    subs = pysubs2.load(srt_path, encoding="utf-8")
    
    color_style  = pick_contrasting_subtitle_style(image_path)
    print(color_style)

    # Apply global style similar to YouTube Shorts
    style = subs.styles["Default"]
    style.fontname = "Arial"
    style.fontsize = 18
    style.bold = True
    style.primarycolor = pysubs2.Color(*color_style["primary"])
    style.outlinecolor = pysubs2.Color(*color_style["outline"])
    style.backcolor = pysubs2.Color(0, 0, 0, 128)       # Semi-transparent bg
    style.outline = 2
    style.shadow = 1
    style.alignment = pysubs2.Alignment.MIDDLE_CENTER

    subs.save(ass_path)
    print(f"Saved styled ASS to: {ass_path}")

def create_base_video(image_path, audio_path, duration, tmp_video_path, music_path=None):
    print("Creating base video...")
    audio = AudioFileClip(audio_path)
    
    if music_path:
        music_audio = AudioFileClip(music_path).with_effects([afx.MultiplyVolume(0.2)])
        final_audio = CompositeAudioClip([music_audio, audio])
    else:
        final_audio = audio

    image_clip = ImageClip(image_path).with_duration(duration)
    
    # Zoom in over duration    
    zoom_factor = random.uniform(0.1, 0.3)

    zoomed = image_clip.with_effects([vfx.Resize(lambda t: 1 + zoom_factor * (t / duration))])
    
    # # Optional: Pan by cropping from left to right over duration
    # panned = zoomed.with_effects([
    #     vfx.Crop(x1=lambda t: int(zoomed.w * 0.1 * (t / duration)))  # crop starts shifting horizontally
    # ])
    
    image_clip = zoomed


    video = CompositeVideoClip([image_clip]).with_audio(final_audio)
    video.write_videofile(tmp_video_path, fps=24, codec="libx264")
    return tmp_video_path

def burn_subtitles_ffmpeg(input_path, ass_path, output_path):
    print("Burning ASS subtitles with ffmpeg...")
    cmd = [
        "ffmpeg",
        "-y",  # Overwrite output
        "-i", input_path,
        "-vf", f"ass={ass_path}",
        "-c:a", "copy",  # Keep original audio
        output_path
    ]
    subprocess.run(cmd, check=True)
    print(f"Final video saved: {output_path}")

def create_video(image_path, srt_path, audio_path, output_path, music_path=None):
    tmp_video_path = "temp_video.mp4"
    ass_path = "styled_subs.ass"

    # Parse subtitles and determine final duration
    subtitles = parse_srt_file(srt_path)
    final_duration = max(sub.end.total_seconds() for sub in subtitles)

    # Step 1: Generate base video
    create_base_video(image_path, audio_path, final_duration, tmp_video_path, music_path)

    # Step 2: Convert subtitles to styled ASS
    convert_srt_to_ass(srt_path, ass_path, image_path)

    # Step 3: Burn ASS into final video
    burn_subtitles_ffmpeg(tmp_video_path, ass_path, output_path)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Create a video from image, audio, and SRT subtitles with styled rendering.")
    parser.add_argument("--image", required=True, help="Path to the image file")
    parser.add_argument("--srt", required=True, help="Path to the SRT subtitles file")
    parser.add_argument("--audio", required=True, help="Path to the audio file")
    parser.add_argument("--music", required=False, help="Optional background music audio file")
    parser.add_argument("--output", default="output.mp4", help="Output video path (default: output.mp4)")

    args = parser.parse_args()
    create_video(args.image, args.srt, args.audio, args.output,args.music)
