# CI 专用 Unity Android 镜像:game-ci 基础镜像 + Gradle 8.11.1 + JDK 17
# 用途:AGP 8.9.3 要求 Gradle >= 8.11.1 且 JVM >= 17,而 Unity 2022.3 自带
#       Gradle 7.5.1 + OpenJDK 11,且 Gradle 版本选择只认 EditorPrefs(机器级),
#       所以在镜像层把 Unity "自带" 的工具替换掉。
# 构建/推送走 .github/workflows/build-ci-image.yml
FROM unityci/editor:ubuntu-2022.3.62f3-android-3

USER root

# Gradle 8.11.1 顶替 Unity 自带的 7.5.1(路径不变,Unity 的
# "Gradle Installed with Unity" 直接解析到新版,生成的 wrapper 也是 8.11.1)
RUN apt-get update \
 && apt-get install -y --no-install-recommends curl unzip \
 && curl -fsSL https://services.gradle.org/distributions/gradle-8.11.1-bin.zip -o /tmp/gradle.zip \
 && rm -rf /opt/unity/Editor/Data/PlaybackEngines/AndroidPlayer/Tools/gradle \
 && unzip -q /tmp/gradle.zip -d /opt/unity/Editor/Data/PlaybackEngines/AndroidPlayer/Tools/ \
 && mv /opt/unity/Editor/Data/PlaybackEngines/AndroidPlayer/Tools/gradle-8.11.1 \
       /opt/unity/Editor/Data/PlaybackEngines/AndroidPlayer/Tools/gradle \
 && rm /tmp/gradle.zip \
 && rm -rf /var/lib/apt/lists/*

# JDK 17(AGP 8.x 硬性要求;路径写入 gradleTemplate.properties 的 org.gradle.java.home)
RUN apt-get update \
 && apt-get install -y --no-install-recommends openjdk-17-jdk-headless \
 && rm -rf /var/lib/apt/lists/*
