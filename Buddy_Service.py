import os
import platform
import subprocess
import threading
import time

import psutil

from Buddy_Logic import run_compute_shards


class AdaptiveBuddy:
    def __init__(self):
        self.is_server = self.detect_server_os()
        self.process = psutil.Process(os.getpid())

    def detect_server_os(self):
        """Detects if running on Windows Server or high-core count hardware."""
        if platform.system() != "Windows":
            return False
        try:
            output = subprocess.check_output("wmic os get caption", shell=True).decode()
            cpu_count = psutil.cpu_count(logical=False) or 0
            if "Server" in output or cpu_count > 24:
                return True
            return False
        except Exception:
            return False

    def disable_throttling(self):
        """Forces the hardware to stay at max clock speeds."""
        print("[SYSTEM] Server detected. Disabling CPU Throttling...")
        try:
            self.process.nice(psutil.HIGH_PRIORITY_CLASS)
            subprocess.run(
                "powercfg /setactive e9a42b02-d5df-448d-aa00-03f14749eb61",
                shell=True,
                check=False,
            )
            subprocess.run("powercfg /SETPROCESSORTHROTTLEVALUE 0", shell=True, check=False)
            print("[SUCCESS] Speed limiters removed. Full Trawl engaged.")
        except Exception as exc:
            print(f"[WARN] Performance override failed: {exc}")

    def start_service(self):
        print(f"[THE FORGE] Status: {'GHOST (Server)' if self.is_server else 'VISUAL (Workstation)'}")
        if self.is_server:
            self.disable_throttling()
            threading.Thread(target=self.compute_loop, daemon=True).start()
            while True:
                time.sleep(3600)
        else:
            threading.Thread(target=self.compute_loop, daemon=True).start()
            self.run_visual_sentry()

    def compute_loop(self):
        """The actual fishing logic."""
        while True:
            run_compute_shards()
            time.sleep(1)

    def run_visual_sentry(self):
        """Placeholder for workstation UI loop."""
        while True:
            time.sleep(1)


if __name__ == "__main__":
    buddy = AdaptiveBuddy()
    buddy.start_service()
