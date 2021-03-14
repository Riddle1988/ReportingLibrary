$(document).ready(function ()
{
    $('.table-div').on('change','select',function(ev) {
        var data = $(this).closest('table').attr('id')
        var filters=[];
        $(".filterText[id^=" + data + "]").each((k,s)=>{
          if (s.value!='All')                                    // exclude "all" as a non-selection
            filters.push([$(s).closest('th').index(),s.value]);  // return: [positon, filtertext]
        });
        $(".content[id^=" + data + "]").each(function(i,tr){
          let tds=$('td',tr).map((j,td)=>td.textContent).toArray(); // collects individual TD texts
          $(tr).toggle( filters.every(f=>tds[f[0]].search(f[1])>-1 ) ); // show or hide!
        });
      });

    const selectSuite = new mdc.select.MDCSelect(document.querySelector('.mdc-select'));
    selectSuite.listen('MDCSelect:change', () => {
        var testsuites = document.getElementsByClassName("testsuite-card")
        for (var i = 0; i < testsuites.length; i++) {
            if(testsuites[i].id == selectSuite.value) {
                testsuites[i].style.display = "flex";
            }
            else {
                testsuites[i].style.display = "none";
            }
        }
    });
    selectSuite.selectedIndex = 0;

    // Toggle display of test step
    $("input.teststep-checkbox").click(function () {
        toggleDetails($(this).attr("id"));
    });

    // Toggle display of Testlogs
    $("input.testlog-checkbox").click(function () {
        toggleTestlog($(this).attr("id"));
    });

    // Toggle Testcases
    $("input.testsuite-toggle-testcases-checkbox").click(function () {
        // Because ".is-checked" is only set after the event, this is counterintuitive
        if ($(this).parent().is(".is-checked")) {   // If unchecked
            $("label.teststep-checkbox[id^=" + $(this).attr("suiteid") + "]").each(function () {
                if ($(this).is(".is-checked") == true) {
                    toggleDetails($(this).find("input").attr('id'));
                }
            });
        } else {    // If checked
            $("label.teststep-checkbox[id^=" + $(this).attr("suiteid") + "]").each(function () {
                if ($(this).is(".is-checked") == false) {
                    toggleDetails($(this).find("input").attr('id'));
                }
            });
        }
    });

    // Toggle showing Test Definitions
    $("input.testsuite-toggle-testdefs-checkbox").click(function () {
        var divstring = "div#" + $(this).attr('id') + "_div";
        $(divstring).toggle();
    });

    $("input.testsuite-toggle-all-checkbox").click(function () {
        // Because ".is-checked" is only set after the event, this is counterintuitive
        if ($(this).parent().is(".is-checked")) {   // If unchecked
          // Hide Test Definitions
          var td = $("label.testsuite-toggle-testdefs-checkbox[suiteid=" + $(this).attr('suiteid') + "]")[0];
          var divstring = "div#" + $(td).find("input").attr('id') + "_div";
          $(divstring).toggle(false);

          // Hide all Teststeps
          var ts = $("label.testsuite-toggle-testcases-checkbox[suiteid=" + $(this).attr('suiteid') + "]")[0];

          var testsuiteid = $(this).attr("suiteid");
          $("label.teststep-checkbox[id^=" + $(this).attr("suiteid") + "]").each(function () {
              toggleDetailsBool($(this).find("input").attr('id'), false);
          });

          // Fold all testcases
          $(".testcase-card[id^=" + testsuiteid + "]").children(":not(.testcase-title,.testcase-subtitle)").toggle(false);

          // Hide all testlogs
          $("label.testlog-checkbox[for^=" + $(this).attr("suiteid") + "]").each(function () {
            if($(this).is(".is-checked")) {
              var testlogstring = $(this).attr("for");
              toggleTestlog(testlogstring);
            }
          });
        }
        else {
          // Show Test Definitions
          var td = $("label.testsuite-toggle-testdefs-checkbox[suiteid=" + $(this).attr('suiteid') + "]")[0];
          var divstring = "div#" + $(td).find("input").attr('id') + "_div";
          $(divstring).toggle(true);

          // Show all Teststeps
          var ts = $("label.testsuite-toggle-testcases-checkbox[suiteid=" + $(this).attr('suiteid') + "]")[0];

          var testsuiteid = $(this).attr("suiteid");
          $("label.teststep-checkbox[id^=" + $(this).attr("suiteid") + "]").each(function () {
              toggleDetailsBool($(this).find("input").attr('id'), true);
          });

          // Show all testlogs
          $("label.testlog-checkbox[for^=" + $(this).attr("suiteid") + "]").each(function () {

              var testlogstring = $(this).attr("for");
              if ($("span#" + testlogstring + "_span").is(":empty") || $("table#" + testlogstring + "_table").is(":hidden")) {
                  toggleTestlog(testlogstring);
              }
          });

          // Unfold all testcases
          $(".testcase-card[id^=" + testsuiteid + "]").children(":not(.testcase-title,.testcase-subtitle)").toggle(true);
        }
    });

    // Jump down to testlog
    $("a.verdictlink").click(function () {
        var testlogstring = $(this).attr("testlog");
        var linktostring = $(this).attr("linkto");

        // Open testlog if closed
        if ($("span#" + testlogstring + "_span").is(":empty") || $("table#" + testlogstring + "_table").is(":hidden")) {
            toggleTestlog(testlogstring);
        }

        // Highlight the verdict field to indicate something happened
        $(this).parent().effect("highlight", { queue: false }, 5000);

        // Dirty hack. It takes some time for the layout to update, so we keep polling until that is done, and THEN move the screen to the desired log.
        var interval = window.setInterval(function () {
            if ($("table#" + testlogstring + "_table").is(":hidden")) {
            }
            else {
                $('html,body').animate({ scrollTop: $("#" + linktostring).offset().top - $(window).height() / 2 }, 'slow');
                $("#" + linktostring).effect("highlight", { queue: false }, 5000);
                window.clearInterval(interval);
            }
        }, 1);
    });

    // Jump down to testcase
    $("a.verdicttablelink").click(function () {
        var linktostring = $(this).attr("linkto");
        toggleTestcase(linktostring, true);

        // Highlight the verdict field to indicate something happened
        $(this).parent().effect("highlight", { queue: false }, 5000);
        $('html,body').animate({ scrollTop: $("#" + linktostring).offset().top - $(window).height() * 0.1 }, 'slow');
    });

    // Toggle testcase
    $(".testcase-title-h3").click(function () {
        toggleTestcase($(this).parent().parent().attr("id"));
    });

    // Back to top button
    $("a.back-to-top").click(function () {
        var target = $(this).attr("linkto");
        $('html,body').animate({ scrollTop: $("#" + target).offset().top - $(window).height() * 0.1 }, 'slow', function () {
            $("#" + target).effect("highlight", { queue: false }, 5000);
        });
    });

});

function toggleDetails(id) {
    var divstring = "div#" + id + "_div";
    var trstring = "tr#" + id + "_tr";
    var thstring = "th#" + id + "_th";
    var tdstring = "td#" + id + "_td";

    $(divstring).toggle();
    $(trstring).toggle();
    $(thstring).toggle();
    $(tdstring).toggle();
}

function toggleDetailsBool(id, setBool) {
    var divstring = "div#" + id + "_div";
    var trstring = "tr#" + id + "_tr";
    var thstring = "th#" + id + "_th";
    var tdstring = "td#" + id + "_td";

    $(divstring).toggle(setBool);
    $(trstring).toggle(setBool);
    $(thstring).toggle(setBool);
    $(tdstring).toggle(setBool);
}

function toggleTestlog(id) {
    var testlogspan = "span#" + id + "_span";
    var momentstring = "div#" + id + "_moment";
    var tablestring = "table#" + id + "_table";

    // If the span has the external attribute set to "true", load from external. Otherwise just toggle it.
    if ($(testlogspan).attr("external") == "true") {
        $(momentstring).toggle();
        if ($(testlogspan).is(":empty")) {
            $(testlogspan).load($(testlogspan).attr("prefix") + "__" + id + ".html", null, function () {
                $(tablestring).delay(1000).toggle();
                $(momentstring).toggle();
            });
        }
        else {
            $(testlogspan).empty();
            $(momentstring).toggle();
        }
    }
    else {
        $(tablestring).toggle();
    }
};

function toggleTestcase(id, show) {
    a = typeof a !== 'undefined' ? a : true;
    $("#" + id).children(":not(.testcase-title,.testcase-subtitle)").toggle(show);
};
