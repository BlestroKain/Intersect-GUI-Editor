import argparse
import json
import sys
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


def parse_args():
    parser = argparse.ArgumentParser(description="Render a wireframe PNG from layout JSON.")
    parser.add_argument("--input", required=True, help="Path to layout JSON file.")
    parser.add_argument("--output", required=True, help="Path to output PNG file.")
    return parser.parse_args()


def load_layout(path: Path):
    with path.open("r", encoding="utf-8") as handle:
        return json.load(handle)


def main():
    args = parse_args()
    input_path = Path(args.input)
    output_path = Path(args.output)

    if not input_path.exists():
        raise SystemExit(f"Input file does not exist: {input_path}")

    layout = load_layout(input_path)
    nodes = layout.get("nodes", [])
    canvas = layout.get("canvas", {})
    width = max(int(canvas.get("width", 800)), 1)
    height = max(int(canvas.get("height", 600)), 1)

    image = Image.new("RGB", (width, height), color=(245, 245, 245))
    draw = ImageDraw.Draw(image)

    try:
        font = ImageFont.load_default()
    except Exception:  # noqa: BLE001
        font = None

    for node in nodes:
        computed = node.get("computed", {})
        x = int(computed.get("x", 0))
        y = int(computed.get("y", 0))
        w = int(computed.get("width", 0))
        h = int(computed.get("height", 0))
        if w <= 0 or h <= 0:
            continue

        draw.rectangle([x, y, x + w, y + h], outline=(64, 64, 64), width=2)
        name = node.get("name", "")
        if name:
            draw.text((x + 4, y + 4), name, fill=(32, 32, 32), font=font)

    output_path.parent.mkdir(parents=True, exist_ok=True)
    image.save(output_path, format="PNG")
    return 0


if __name__ == "__main__":
    sys.exit(main())
