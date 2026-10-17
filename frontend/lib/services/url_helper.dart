// Helper to launch browser URLs cleanly across Web and Mobile platforms
import 'url_helper_stub.dart'
    if (dart.library.html) 'url_helper_web.dart';

class UrlHelper {
  static void openUrl(String url) {
    launchWebUrl(url);
  }
}
