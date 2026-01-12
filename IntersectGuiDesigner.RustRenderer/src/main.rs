use serde::Deserialize;
use std::env;
use std::fs;
use std::path::PathBuf;

#[derive(Debug, Deserialize)]
struct LayoutDocument {
    #[serde(default)]
    canvas: LayoutCanvas,
    #[serde(default)]
    nodes: Vec<LayoutNode>,
    #[serde(default)]
    schema_version: Option<String>,
}

#[derive(Debug, Deserialize, Default)]
struct LayoutCanvas {
    #[serde(default = "default_width")]
    width: i32,
    #[serde(default = "default_height")]
    height: i32,
}

#[derive(Debug, Deserialize)]
struct LayoutNode {
    #[serde(default)]
    name: String,
    #[serde(default)]
    computed: LayoutRect,
    #[serde(default)]
    dock: Option<String>,
    #[serde(default)]
    padding: Option<LayoutThickness>,
}

#[derive(Debug, Deserialize, Default)]
struct LayoutRect {
    #[serde(default)]
    x: i32,
    #[serde(default)]
    y: i32,
    #[serde(default)]
    width: i32,
    #[serde(default)]
    height: i32,
}

#[derive(Debug, Deserialize)]
struct LayoutThickness {
    left: i32,
    top: i32,
    right: i32,
    bottom: i32,
}

fn default_width() -> i32 {
    800
}

fn default_height() -> i32 {
    600
}

fn main() {
    let args: Vec<String> = env::args().collect();
    let input = get_arg_value(&args, "--input");
    let output = get_arg_value(&args, "--output");

    let input_path = match input {
        Some(path) => PathBuf::from(path),
        None => exit_with("Missing --input argument"),
    };

    let output_path = match output {
        Some(path) => PathBuf::from(path),
        None => exit_with("Missing --output argument"),
    };

    let data = fs::read_to_string(&input_path)
        .unwrap_or_else(|_| exit_with("Failed to read input JSON"));
    let document: LayoutDocument =
        serde_json::from_str(&data).unwrap_or_else(|_| exit_with("Invalid JSON layout file"));

    let svg = render_svg(&document);
    if let Some(parent) = output_path.parent() {
        if !parent.as_os_str().is_empty() {
            let _ = fs::create_dir_all(parent);
        }
    }

    fs::write(&output_path, svg).unwrap_or_else(|_| exit_with("Failed to write SVG output"));
}

fn get_arg_value(args: &[String], name: &str) -> Option<String> {
    args.iter()
        .position(|value| value == name)
        .and_then(|index| args.get(index + 1))
        .cloned()
}

fn render_svg(document: &LayoutDocument) -> String {
    let width = document.canvas.width.max(1);
    let height = document.canvas.height.max(1);

    let mut output = String::new();
    output.push_str(
        format!(
            r#"<?xml version="1.0" encoding="UTF-8"?>
<svg xmlns="http://www.w3.org/2000/svg" width="{width}" height="{height}" viewBox="0 0 {width} {height}">
  <rect x="0" y="0" width="{width}" height="{height}" fill="#F5F5F5" />
"#
        )
        .as_str(),
    );

    for node in &document.nodes {
        let rect = &node.computed;
        if rect.width <= 0 || rect.height <= 0 {
            continue;
        }

        output.push_str(
            format!(
                r#"  <g>
    <rect x="{x}" y="{y}" width="{w}" height="{h}" fill="#CBD5E1" stroke="#2B6CB0" stroke-width="1" />
"#,
                x = rect.x,
                y = rect.y,
                w = rect.width,
                h = rect.height
            )
            .as_str(),
        );

        if !node.name.is_empty() {
            output.push_str(
                format!(
                    r#"    <text x="{x}" y="{y}" fill="#1A202C" font-size="12" font-family="Segoe UI, Arial, sans-serif">{name}</text>
"#,
                    x = rect.x + 4,
                    y = rect.y + 16,
                    name = escape_text(&node.name)
                )
                .as_str(),
            );
        }

        if let Some(dock) = &node.dock {
            output.push_str(
                format!(
                    r#"    <text x="{x}" y="{y}" fill="#4A5568" font-size="10" font-family="Segoe UI, Arial, sans-serif">Dock: {dock}</text>
"#,
                    x = rect.x + 4,
                    y = rect.y + 30,
                    dock = escape_text(dock)
                )
                .as_str(),
            );
        }

        if let Some(padding) = &node.padding {
            output.push_str(
                format!(
                    r#"    <text x="{x}" y="{y}" fill="#4A5568" font-size="10" font-family="Segoe UI, Arial, sans-serif">Padding: {left},{top},{right},{bottom}</text>
"#,
                    x = rect.x + 4,
                    y = rect.y + 44,
                    left = padding.left,
                    top = padding.top,
                    right = padding.right,
                    bottom = padding.bottom
                )
                .as_str(),
            );
        }

        output.push_str("  </g>\n");
    }

    output.push_str("</svg>\n");
    output
}

fn escape_text(value: &str) -> String {
    value
        .replace('&', "&amp;")
        .replace('<', "&lt;")
        .replace('>', "&gt;")
        .replace('"', "&quot;")
        .replace('\'', "&apos;")
}

fn exit_with(message: &str) -> ! {
    eprintln!("{message}");
    std::process::exit(1);
}
