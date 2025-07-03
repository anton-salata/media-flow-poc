import argparse
from aeneas.executetask import ExecuteTask
from aeneas.task import Task

def generate_subtitles(audio_file, text_file, srt_output):
    #config_string = "task_language=eng|os_task_file_format=srt|is_text_type=plain"
    config_string = (
    "task_language=eng|"
    "os_task_file_format=srt|"
    "is_text_type=plain|"
    "task_adjust_boundary_algorithm=percent|"
    "task_adjust_boundary_percent_value=50"
)
    task = Task(config_string=config_string)

    task.audio_file_path_absolute = audio_file
    task.text_file_path_absolute = text_file
    task.sync_map_file_path_absolute = srt_output

    ExecuteTask(task).execute()
    task.output_sync_map_file()
    print(f"Subtitles saved to: {srt_output}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--audio", required=True, help="Path to audio file (e.g., narration.wav)")
    parser.add_argument("--text", required=True, help="Path to transcript text file (e.g., transcript.txt)")
    parser.add_argument("--srt", required=True, help="Path to output .srt file (e.g., subtitles.srt)")

    args = parser.parse_args()
    generate_subtitles(args.audio, args.text, args.srt)
