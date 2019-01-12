package api.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletRequest;
import java.util.HashMap;
import java.util.Map;

@RestController
public class ApiController {

    @Autowired
    private HttpServletRequest request;

    @GetMapping("/")
    public Object Get() {
        Map map = new HashMap();
        map.put("user", request.getUserPrincipal());
        map.put("author", "lnhcode@outlook.com");
        map.put("github", "https://github.com/linianhui/oidc.example");
        map.put("text", "this is a java api");
        return map;
    }
}
