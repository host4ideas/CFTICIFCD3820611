## Docker commands

#### COPY:

Examples:

- Copy all files starting with "hom":

  ```
  COPY hom* /mydir/
  ```

- The <dest> is the path to WORKDIR, into which thre source will be copied inside the destination container.

  In this example adds "test.txt" to <WORKDIR>/relativeDir/:

  ```
  COPY test.txt relativeDir/
  ```

  In this other example adds "test.txt" to /absoluteDir/:

  ```
  COPY test.txt /absoluteDir/
  ```


#### FROM:

The `FROM` instruction initializes a new build stage and sets the *Base Image* for subsequent instructions. As such, a valid `Dockerfile` must start with a `FROM` instruction.
A base image has no parent image specified in its Dockerfile. It is created using a Dockerfile with the `FROM scratch` directive.



- 



