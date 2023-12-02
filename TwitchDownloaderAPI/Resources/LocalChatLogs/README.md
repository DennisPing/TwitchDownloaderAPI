# Useful Command Line Tools

## Read txt file
```txt
less <chat_log.txt>
```

## Read json file
```txt
jq '.' <chat_log.json> | less
```

## Less commands

Scroll forward and backwards: `j` and `k`

Jump forward and backwards one page: `spacebar` and `b`

Search forward: `/`

Search backwards: `?`

Next occurrence: `n`