# Meta Documentation: execution process

This document will give a walkthrough of the process CSBL uses to execute an input file. It will not provide in-depth technical details, but rather, a broad outline.

The basic flow of a CSBL program looks something like this:

1. **Initialization stage** - responsible for reading the provided command line parameters and opening up a CSBL file. This stage performs no checks of any kind other than validating that there is at least one provided command line argument and the provided file is a valid file of the type `.csbl`.

2. **Preprocessing stage** - responsible for processing a variety of directives, namely `#import`s and interpretation options (those being `ENABLE_REIMPORT_ERROR`, `ENABLE_STAGE_TIMING_OUTPUT`, and `ENABLE_STAGE_EXIT_ERROR` respectively). It is also responsible for combining together all files into one single output string that the tokenizer can properly process. This stage performs no checks of any kind.

3. **Tokenization stage** - responsible for generating a single list of tokens based on the regular expression rules provided to it. Unlike the previous stages, the tokenization stage perform checks to detect invalid characters based on a the remnants of the output string passed to it.

4. **Transformation stage** - responsible for generating a single list of *transformed* tokens based on the previously generated list of tokens. Transformation involves transforming input tokens into their respective types (i.e, string tokens to actual string values, number tokens to actual number values, keyword tokens to their respective enumerated values, etc.). This stage does not check for any invalid tokens. Checks for invalid functions are performed by the interpreter.

5. **Interpretation stage** - responsible for taking the list of transformed tokens and executing them, usually generating an output of some kind. This stage will check for a whole host of errors (e.g, invalid funtions, type mismatchs, stack errors, etc.).
