import whisper
import sys

def format_timestamp(seconds: float) -> str:
    hours = int(seconds // 3600)
    minutes = int((seconds % 3600) // 60)
    secs = int(seconds % 60)
    millis = int((seconds - int(seconds)) * 1000)
    return f"{hours:02}:{minutes:02}:{secs:02},{millis:03}"

def resegment_words(segments, max_words=5, max_duration=3.0):
    new_segments = []
    for seg in segments:
        words = seg.get("words", [])
        if not words:
            continue

        buffer = []
        start_time = words[0]["start"]

        for i, word in enumerate(words):
            buffer.append(word)
            duration = word["end"] - start_time

            is_last_word = i == len(words) - 1
            if len(buffer) >= max_words or duration >= max_duration or is_last_word:
                text = " ".join(w["word"].strip() for w in buffer).strip()
                if text:
                    new_segments.append({
                        "start": start_time,
                        "end": buffer[-1]["end"],
                        "text": text
                    })
                if not is_last_word:
                    start_time = word["end"]
                    buffer = []

    return new_segments

def write_srt(segments, file_path):
    with open(file_path, "w", encoding="utf-8") as f:
        for i, seg in enumerate(segments, start=1):
            start = format_timestamp(seg["start"])
            end = format_timestamp(seg["end"])
            f.write(f"{i}\n{start} --> {end}\n{seg['text'].strip()}\n\n")

def main():
    if len(sys.argv) != 3:
        print("Usage: python whisper_srt_chunked.py <input_audio.wav> <output_subtitles.srt>")
        sys.exit(1)

    audio_path = sys.argv[1]
    srt_path = sys.argv[2]

    print("Loading Whisper model...")
    model = whisper.load_model("base")  # or "small", "medium", "large"

    print("Transcribing with word-level timestamps...")
    result = model.transcribe(
        audio_path,
        task="transcribe",
        word_timestamps=True,
        verbose=True
    )

    print("Resegmenting for better subtitle readability...")
    short_segments = resegment_words(result["segments"], max_words=5, max_duration=3.0)

    print(f"Writing subtitles to {srt_path}...")
    write_srt(short_segments, srt_path)
    print("Done!")

if __name__ == "__main__":
    main()
