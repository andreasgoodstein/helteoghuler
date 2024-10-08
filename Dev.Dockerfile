# Based on Debian 12
FROM mcr.microsoft.com/dotnet/sdk:8.0 

# Install Docker CLI
RUN apt-get update
RUN apt-get install -y ca-certificates curl ssh git
RUN install -m 0755 -d /etc/apt/keyrings
RUN curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
RUN chmod a+r /etc/apt/keyrings/docker.asc
RUN echo \
    "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/debian \
    $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
    tee /etc/apt/sources.list.d/docker.list > /dev/null
RUN apt-get update
RUN apt-get install -y docker-ce-cli

# Install oh-my-bash
RUN bash -c "$(curl -fsSL https://raw.githubusercontent.com/ohmybash/oh-my-bash/master/tools/install.sh)"

# Setup non-root user
ARG REMOTE_USER
ARG USER_UID=1000
ARG USER_GID=$USER_UID

RUN groupadd --gid $USER_GID $REMOTE_USER
RUN useradd --uid $USER_UID --gid $USER_GID -m $REMOTE_USER

ENV HOME=/home/$REMOTE_USER

# [Optional] Add sudo to non-root user
# RUN apt-get install -y sudo \
#     && echo $REMOTE_USER ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$REMOTE_USER \
#     && chmod 0440 /etc/sudoers.d/$REMOTE_USER

USER $REMOTE_USER
