#!/bin/sh

is_wip_commit_subject() {
  subject="$1"
  printf '%s' "$subject" | grep -Eiq '^wip([[:space:]:_-]|$)'
}

should_skip_heavy_checks() {
  commit_msg_file="$1"

  if [ ! -f "$commit_msg_file" ]; then
    return 1
  fi

  first_line="$(head -n 1 "$commit_msg_file")"
  if is_wip_commit_subject "$first_line"; then
    printf '\033[33m%s\033[0m\n' "WIP commit detected. Skipping: format, build, test."
    return 0
  fi

  return 1
}
