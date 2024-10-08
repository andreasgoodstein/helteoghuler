# Based on Debian 12
FROM mcr.microsoft.com/dotnet/sdk:8.0 

# Setup package repositories
RUN apt-get update
RUN apt-get install -y dirmngr gnupg ca-certificates curl ssh git
RUN install -m 0755 -d /etc/apt/keyrings
RUN curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
RUN chmod a+r /etc/apt/keyrings/docker.asc
RUN echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/debian $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null
RUN gpg --homedir /tmp --no-default-keyring --keyring /usr/share/keyrings/mono-official-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb [signed-by=/usr/share/keyrings/mono-official-archive-keyring.gpg] https://download.mono-project.com/repo/debian stable-buster main" | tee /etc/apt/sources.list.d/mono-official-stable.list
RUN apt-get update

# Install Docker CLI & Mono
RUN apt-get install -y docker-ce-cli mono-devel


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
