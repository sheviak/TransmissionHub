#!/usr/bin/env sh

# Default target is master if not provided
TARGET_REF=${1:-master}

# Collect changed files.
# We combine BOTH the committed changes (master...HEAD) AND the currently staged ones (--cached).
# In a local commit hook, HEAD doesn't contain the currently staged files yet.
# In GitHub Actions, currently staged files are empty, but HEAD contains the PR commits.
CHANGED_FILES=$( { git diff --name-only "$TARGET_REF"...HEAD 2>/dev/null; git diff --cached --name-only 2>/dev/null; } | sort -u )

touches_lib=false
touches_bot=false

for file in $CHANGED_FILES; do
  case "$file" in
    lib/*)
      touches_lib=true
      ;;
    bot/*)
      touches_bot=true
      ;;
  esac
done

if [ "$touches_lib" = true ] && [ "$touches_bot" = true ]; then
  echo "both"
elif [ "$touches_lib" = true ]; then
  echo "lib"
elif [ "$touches_bot" = true ]; then
  echo "bot"
else
  echo "none"
fi
